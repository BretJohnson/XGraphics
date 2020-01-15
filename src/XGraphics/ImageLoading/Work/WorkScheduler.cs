using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using XGraphics.ImageLoading.Concurrency;
using XGraphics.ImageLoading.Extensions;
using XGraphics.ImageLoading.Helpers;

namespace XGraphics.ImageLoading.Work
{
    public class WorkScheduler : IWorkScheduler
    {
        private readonly ImageLoader _imageLoader;
        private readonly object _lock = new object();
        private long _statsTotalPending;
        private long _statsTotalRunning;
        private long _statsTotalMemoryCacheHits;
        private long _statsTotalWaiting;
        private long _loadCount;

        public WorkScheduler(ImageLoader imageLoader)
        {
            _imageLoader = imageLoader;
        }

        protected int MaxParallelTasks => _imageLoader.SchedulerMaxParallelTasks;

        protected PendingTasksQueue PendingTasks { get; } = new PendingTasksQueue();
        protected Dictionary<string, IImageLoaderTask> RunningTasks { get; } = new Dictionary<string, IImageLoaderTask>();
        protected ThreadSafeCollection<IImageLoaderTask> SimilarTasks { get; } = new ThreadSafeCollection<IImageLoaderTask>();

        public virtual void Cancel(Func<IImageLoaderTask, bool> predicate)
        {
            lock (_lock)
            {
                foreach (var task in PendingTasks.Where(p => predicate(p)))
                {
                    task?.Cancel();
                }

                SimilarTasks.RemoveAll(predicate);
            }
        }

        public bool ExitTasksEarly { get; private set; }

        public void SetExitTasksEarly(bool exitTasksEarly)
        {
            if (ExitTasksEarly == exitTasksEarly)
                return;

            ExitTasksEarly = exitTasksEarly;

            if (exitTasksEarly)
            {
                _imageLoader.Logger?.Debug("ExitTasksEarly enabled.");

                lock (_lock)
                {
                    foreach (var task in PendingTasks)
                        task?.Cancel();

                    PendingTasks.Clear();

                    foreach (var task in SimilarTasks)
                        task?.Cancel();

                    SimilarTasks.Clear();
                }
            }
            else
            {
                _imageLoader.Logger?.Debug("ExitTasksEarly disabled.");
            }
        }

        public bool PauseWork { get; private set; }

        public void SetPauseWork(bool pauseWork, bool cancelExisting = false)
        {
            if (PauseWork == pauseWork)
                return;

            if (cancelExisting)
            {
                lock (_lock)
                {
                    foreach (var task in PendingTasks)
                        task?.Cancel();

                    PendingTasks.Clear();

                    foreach (var task in SimilarTasks)
                        task?.Cancel();

                    SimilarTasks.Clear();
                }
            }

            PauseWork = pauseWork;

            if (pauseWork)
            {
                _imageLoader.Logger?.Debug("SetPauseWork enabled.");
            }
            else
            {
                _imageLoader.Logger?.Debug("SetPauseWork disabled.");
                TakeFromPendingTasksAndRun();
            }
        }

        public virtual void RemovePendingTask(IImageLoaderTask task)
        {
            if (task != null)
            {
                lock (_lock)
                {
                    PendingTasks.TryRemove(task);
                    SimilarTasks.Remove(task);
                }
            }
        }

        public virtual void LoadImage(IImageLoaderTask task)
        {
            try
            {
                Interlocked.Increment(ref _loadCount);

                if (task == null)
                    return;

                if (task.IsCancelled || task.IsCompleted || ExitTasksEarly)
                {
                    if (!task.IsCompleted)
                        task.TryDispose();
                    return;
                }

                if (_imageLoader.VerbosePerformanceLogging && (_loadCount % 10) == 0)
                {
                    LogSchedulerStats(_imageLoader.Performance);
                }

                if (string.IsNullOrWhiteSpace(task.KeyRaw))
                {
                    _imageLoader.Logger?.Error("ImageService: Key cannot be null");
                    task.TryDispose();
                    return;
                }

                // If we have the image in memory then it's pointless to schedule the job: just display it straight away
                if (task.CanUseMemoryCache && task.TryLoadFromMemoryCache())
                {
                    Interlocked.Increment(ref _statsTotalMemoryCacheHits);
                    task.TryDispose();
                    return;
                }

                QueueImageLoadingTask(task);
            }
            catch (Exception ex)
            {
                _imageLoader.Logger?.Error($"Image loaded failed: {task?.Key}", ex);
            }
        }

        private void Enqueue(IImageLoaderTask task)
        {
            int priority = task.Priority ?? task.ImageSource.Source.DefaultLoadingPriority;
            PendingTasks.Enqueue(task, priority);
        }

        protected void QueueImageLoadingTask(IImageLoaderTask task)
        {
            if (task.IsCancelled || task.IsCompleted || ExitTasksEarly)
            {
                if (!task.IsCompleted)
                    task.TryDispose();

                return;
            }

            IImageLoaderTask? similarRunningTask = null;

            similarRunningTask = PendingTasks.FirstOrDefaultByRawKey(task.KeyRaw);
            if (similarRunningTask == null)
            {
                Interlocked.Increment(ref _statsTotalPending);
                Enqueue(task);
            }
            else
            {
                if (task.Priority.HasValue && (!similarRunningTask.Priority.HasValue
                    || task.Priority.Value > similarRunningTask.Priority.Value))
                {
                    similarRunningTask.Priority = task.Priority.Value;
                    PendingTasks.TryUpdatePriority(similarRunningTask, task.Priority.Value);
                }

#if LATER
                similarRunningTask.ImageSource.DownloadProgress += (sender, args) =>
                    task.ImageSource.RaiseDownloadProgress(args);
#endif
            }

            if (PauseWork)
                return;

            if (similarRunningTask == null || !task.CanUseMemoryCache)
            {
                TakeFromPendingTasksAndRun();
            }
            else
            {
                Interlocked.Increment(ref _statsTotalWaiting);
                _imageLoader.Logger?.Debug($"Wait for similar request for key: {task.Key}");
                SimilarTasks.Add(task);
            }
        }

        protected async void TakeFromPendingTasksAndRun()
        {
            await TakeFromPendingTasksAndRunAsync().ConfigureAwait(false);
        }

        protected async Task TakeFromPendingTasksAndRunAsync()
        {
            if (PendingTasks.Count == 0)
                return;

            Dictionary<string, IImageLoaderTask>? tasksToRun = null;

            int urlTasksCount = 0;

            lock (_lock)
            {
                if (RunningTasks.Count >= MaxParallelTasks)
                {
                    urlTasksCount = RunningTasks.Count(v => v.Value != null && v.Value.ImageSource.Source is UriSource);

                    if (urlTasksCount == 0 || urlTasksCount != MaxParallelTasks)
                        return;

                    // Allow only half of MaxParallelTasks as additional allowed tasks when preloading occurs to prevent starvation
                    if (RunningTasks.Count - Math.Max(1, Math.Min(urlTasksCount, MaxParallelTasks / 2)) >= MaxParallelTasks)
                        return;
                }

                int numberOfTasks = MaxParallelTasks - RunningTasks.Count + Math.Min(urlTasksCount, MaxParallelTasks / 2);
                tasksToRun = new Dictionary<string, IImageLoaderTask>();

                while (tasksToRun.Count < numberOfTasks && PendingTasks.TryDequeue(out IImageLoaderTask? task))
                {
                    if (task == null || task.IsCancelled || task.IsCompleted)
                        continue;

                    // We don't want to load, at the same time, images that have same key or same raw key at the same time
                    // This way we prevent concurrent downloads and benefit from caches
                    string rawKey = task.KeyRaw;
                    if (RunningTasks.ContainsKey(rawKey) || tasksToRun.ContainsKey(rawKey))
                    {
                        SimilarTasks.Add(task);
                        continue;
                    }

                    if (urlTasksCount != 0)
                    {
                        if (! (task.ImageSource.Source is UriSource))
                            tasksToRun.Add(rawKey, task);
                        else
                        {
                            Enqueue(task);
                            break;
                        }
                    }
                    else
                    {
                        tasksToRun.Add(rawKey, task);
                    }
                }

                foreach (var item in tasksToRun)
                {
                    RunningTasks.Add(item.Key, item.Value);
                    Interlocked.Increment(ref _statsTotalRunning);
                }
            }

            if (tasksToRun != null && tasksToRun.Count > 0)
            {
                var tasks = tasksToRun.Select(async p =>
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        try
                        {
                            await RunImageLoadingTaskAsync(p.Value).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _imageLoader.Logger?.Error("TakeFromPendingTasksAndRun exception", ex);
                        }
                    }, CancellationToken.None, TaskCreationOptions.PreferFairness | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, TaskScheduler.Default).ConfigureAwait(false);
                });
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        protected async Task RunImageLoadingTaskAsync(IImageLoaderTask pendingTask)
        {
            string keyRaw = pendingTask.KeyRaw;

            try
            {
                if (_imageLoader.VerbosePerformanceLogging)
                {
                    IPlatformPerformance performance = _imageLoader.Performance;

                    LogSchedulerStats(performance);
                    var stopwatch = Stopwatch.StartNew();

                    await pendingTask.RunAsync().ConfigureAwait(false);

                    stopwatch.Stop();

                    _imageLoader.Logger?.Debug(string.Format("[PERFORMANCE] RunAsync - NetManagedThreadId: {0}, NativeThreadId: {1}, Execution: {2} ms, Key: {3}",
                        performance.GetCurrentManagedThreadId(),
                        performance.GetCurrentSystemThreadId(),
                        stopwatch.Elapsed.Milliseconds,
                        pendingTask.Key));
                }
                else
                {
                    await pendingTask.RunAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                lock (_lock)
                {
                    RunningTasks.Remove(keyRaw);

                    if (SimilarTasks.Count > 0)
                    {
                        SimilarTasks.RemoveAll(v => v == null || v.IsCompleted || v.IsCancelled);
                        var similarItems = SimilarTasks.Where(v => v.KeyRaw == keyRaw);
                        foreach (var similar in similarItems)
                        {
                            SimilarTasks.Remove(similar);

                            LoadImage(similar);
                        }
                    }
                }

                pendingTask.TryDispose();
                await TakeFromPendingTasksAndRunAsync().ConfigureAwait(false);
            }
        }

        protected void LogSchedulerStats(IPlatformPerformance performance)
        {
            IMiniLogger? logger = _imageLoader.Logger;
            if (logger == null)
                return;

            logger.Debug(string.Format("[PERFORMANCE] Scheduler - Max: {0}, Pending: {1}, Running: {2}, TotalPending: {3}, TotalRunning: {4}, TotalMemoryCacheHit: {5}, TotalWaiting: {6}",
                        MaxParallelTasks,
                        PendingTasks.Count,
                        RunningTasks.Count,
                        _statsTotalPending,
                        _statsTotalRunning,
                        _statsTotalMemoryCacheHits,
                        _statsTotalWaiting));

            logger.Debug(performance.GetMemoryInfo());
        }

#if ADD_TO_DATASOURCE_SUBCLASSES
        protected virtual int GetDefaultPriority(ILoadableImageSource source)
        {
            if (source == ImageSource.ApplicationBundle || source == ImageSource.CompiledResource) {
                return (int)LoadingPriority.Normal + 2;

            if (source == ImageSource.Filepath)
                return (int)LoadingPriority.Normal + 1;

            return (int)LoadingPriority.Normal;
        }
#endif
    }
}
