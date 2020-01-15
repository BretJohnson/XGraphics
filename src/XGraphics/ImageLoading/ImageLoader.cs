using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Cache;
using XGraphics.ImageLoading.Helpers;
using XGraphics.ImageLoading.Work;

namespace XGraphics.ImageLoading
{
    /// <summary>
    /// Note that some platforms subclass ImageLoader, to make certain functionality platform specific.
    /// </summary>
    public class ImageLoader : IImageLoader
    {
        protected bool _initialized;
        protected bool _isInitializing;
        protected object _initializeLock = new object();
        private IDiskCache? _diskCache;
        private int _decodingMaxParallelTasks;

        /// <summary>
        /// Gets or sets the cache storage type, (Memory, Disk, All). Defaults to All.
        /// </summary>
        /// <value>Cache types to enable</value>
        public CacheType CacheType { get; set; } = CacheType.All;

        /// <summary>
        /// Gets or sets the HTTP client used for web requests. The default HTTP client has a read timeout of 15 seconds.
        /// Update the Timeout property on HttpClient if you wish to change that.
        /// </summary>
        /// <value>The http client used for web requests.</value>
        public HttpClient HttpClient { get; set; } =
            new HttpClient(
                new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                })
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

        /// <summary>
        /// Gets or sets the maximum time in seconds to wait to receive HTTP headers before the HTTP request is cancelled.
        /// </summary>
        /// <value>The http connect timeout.</value>
        public int HttpHeadersTimeout { get; set; } = 3;

        /// <summary>
        /// Gets or sets the size of the http read buffer.
        /// </summary>
        /// <value>The size of the http read buffer.</value>
        public int HttpReadBufferSize { get; set; } = 8192;

        /// <summary>
        /// Gets or sets the number of times to automatically retry if image loading fails.
        /// </summary>
        /// <value>Number of retries</value>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// Gets or sets the delay between automatic retry attempts.
        /// </summary>
        /// <value>Delay in milliseconds between each trial</value>
        public int RetryDelayInMs { get; set; } = 250;

        /// <summary>
        /// Gets or sets the default duration of the disk cache entries.
        /// </summary>
        /// <value>The duration of the cache.</value>
        public TimeSpan DiskCacheDuration { get; set; } = TimeSpan.FromDays(30d);

        /// <summary>
        /// Gets or sets a value indicating whether try to read
        /// disk cache duration from http headers .
        /// </summary>
        /// <value><c>true</c> if try to read disk cache duration from http headers; otherwise, <c>false</c>.</value>
        public bool TryToReadDiskCacheDurationFromHttpHeaders { get; set; } = true;

        /// <summary>
        /// Milliseconds to wait prior to start any task.
        /// </summary>
        public int DelayInMs { get; set; } = 10;   //Task.Delay resolution is around 15ms

        /// <summary>
        /// Gets or sets the scheduler used to organize/schedule image loading tasks.
        /// </summary>
        /// <value>The scheduler used to organize/schedule image loading tasks.</value>
        public IWorkScheduler Scheduler { get; set; }

        /// <summary>
        /// Gets or sets the platform performance object, used to query performance related info.
        /// </summary>
        /// <value>The platform performance object.</value>
        public IPlatformPerformance Performance { get; set; }

        /// <summary>
        /// Gets or sets the logger used to receive debug/error messages.
        /// </summary>
        /// <value>The logger.</value>
        public IMiniLogger? Logger { get; set; } = new ConsoleMiniLogger();

        /// <summary>
        /// Gets or sets the disk cache.
        /// </summary>
        /// <value>The disk cache.</value>
        public IDiskCache DiskCache
        {
            get
            {
                if (_diskCache == null)
                    _diskCache = CreatePlatformDiskCache();
                return _diskCache;
            }

            set => _diskCache = value;
        }

        /// <summary>
        /// Gets or sets the disk cache path. The default is a reasonable path, that's platform dependent.
        /// </summary>
        /// <value>The disk cache path.</value>
        public string DiskCachePath { get; set; }

        /// <summary>
        /// Gets or sets the memory cache.
        /// </summary>
        /// <value>The memory cache.</value>
        public IMemoryCache? MemoryCache { get; set; }

        /// <summary>
        /// Gets or sets the download cache. Download cache is responsible for retrieving data from the web, or taking from the disk cache.
        /// </summary>
        /// <value>The download cache.</value>
        public IDownloadCache DownloadCache { get; set; }

        /// <summary>
        /// Enables / disables verbose performance logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose performance logging.</value>
        public bool VerbosePerformanceLogging { get; set; } = false;

        /// <summary>
        /// Enables / disables verbose memory cache logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose memory cache logging.</value>
        public bool VerboseMemoryCacheLogging { get; set; } = false;

        /// <summary>
        /// Enables / disables verbose image loading cancelled logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose image loading cancelled logging.</value>
        public bool VerboseLoadingCancelledLogging { get; set; } = false;

        /// <summary>
        /// Enables / disables  verbose logging.
        /// IMPORTANT! If it's disabled are other verbose logging options are disabled too
        /// </summary>
        /// <value>The verbose logging.</value>
        public bool VerboseLogging { get; set; } = false;

        /// <summary>
        /// Gets or sets the scheduler max parallel tasks.
        /// Default: Environment.ProcessorCount <= 2 ? 1 : 2;
        /// </summary>
        /// <value>The decoding max parallel tasks.</value>
        public int DecodingMaxParallelTasks
        {
            get => _decodingMaxParallelTasks;
            set
            {
                _decodingMaxParallelTasks = value;
                DecodingLock = new SemaphoreSlim(value, value);
            }
        }

        /// <summary>
        /// Gets or sets the scheduler max parallel tasks.
        /// Default: Math.Max(16, 4 + Environment.ProcessorCount);
        /// </summary>
        /// <value>The scheduler max parallel tasks.</value>
        public int SchedulerMaxParallelTasks { get; set; } = Math.Max(16, 4 + Environment.ProcessorCount);

        /// <summary>
        /// Gets or sets a value indicating whether clear
        /// memory cache on out of memory.
        /// </summary>
        /// <value><c>true</c> if clear memory cache on out of memory; otherwise, <c>false</c>.</value>
        public bool ClearMemoryCacheOnOutOfMemory { get; set; } = true;

        protected ImageLoader()
        {
            // TODO: Have a better default disk cache path, that's app specific on all platforms
            // Note that platforms that support app specific data set their own DiskCachePath default
            string tmpPath = Path.GetTempPath();
            string cachePath = Path.Combine(tmpPath, "XGraphicsDiskCache");
            DiskCachePath = cachePath;

            DecodingMaxParallelTasks = Environment.ProcessorCount <= 2 ? 1 : 2;

            Scheduler = new WorkScheduler(this);

            // Most platform subclasses will reset Performance to a platform appropriate implementation
            Performance = new EmptyPlatformPerformance();

            DownloadCache = new DownloadCache(this);

            DecodingLock = new SemaphoreSlim(DecodingMaxParallelTasks, DecodingMaxParallelTasks);
        }

        /// <summary>
        /// Some platforms override this default
        /// </summary>
        /// <returns>disk cache instance</returns>
        protected virtual IDiskCache CreatePlatformDiskCache()
        {
            return new SimpleDiskCache(this, DiskCachePath);
        }

        public bool PauseWork => Scheduler.PauseWork;

        public SemaphoreSlim DecodingLock { get; private set; }

		public void SetPauseWorkAndCancelExisting(bool pauseWork) => SetPauseWork(pauseWork, true);

		public void SetPauseWork(bool pauseWork, bool cancelExisting = false)
        {
            Scheduler.SetPauseWork(pauseWork, cancelExisting);
        }

        public void CancelWorkFor(IImageLoaderTask task)
        {
            task?.Cancel();
        }

        public void RemovePendingTask(IImageLoaderTask task)
        {
            Scheduler.RemovePendingTask(task);
        }

        public void LoadImage(ILoadableImageSource imageSource, ImageDecoder? decoder)
        {
            var imageLoaderTask = new ImageLoaderTask(this, imageSource, decoder);
            Scheduler.LoadImage(imageLoaderTask);
        }

        public async Task InvalidateCacheAsync(CacheType cacheType)
        {
            if (cacheType == CacheType.All || cacheType == CacheType.Memory)
            {
                InvalidateMemoryCache();
            }

            if (cacheType == CacheType.All || cacheType == CacheType.Disk)
            {
                await InvalidateDiskCacheAsync().ConfigureAwait(false);
            }
        }

        public void InvalidateMemoryCache()
        {
            MemoryCache?.Clear();
        }

        public Task InvalidateDiskCacheAsync()
        {
            if (DiskCache == null)
                return Task.CompletedTask;
            else return DiskCache.ClearAsync();
        }

        public async Task InvalidateCacheEntryAsync(string key, CacheType cacheType, bool removeSimilar = false)
        {
            if (cacheType == CacheType.All || cacheType == CacheType.Memory)
            {
                MemoryCache?.Remove(key);

                if (removeSimilar)
                {
                    MemoryCache?.RemoveSimilar(key);
                }
            }

            if (cacheType == CacheType.All || cacheType == CacheType.Disk)
            {
                string hash = MD5Helper.MD5(key);
                if (DiskCache != null)
                    await DiskCache.RemoveAsync(hash).ConfigureAwait(false);
            }
        }

        public void Cancel(Func<IImageLoaderTask, bool> predicate)
        {
            Scheduler.Cancel(predicate);
        }
    }
}
