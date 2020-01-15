using System;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Cache;

namespace XGraphics.ImageLoading.Work
{
    /// <summary>
    /// FFImageLoading by Daniel Luberda
    /// </summary>
    public interface IImageLoader
    {
        /// <summary>
        /// Gets or sets the download cache. Download cache is responsible for retrieving data from the web, or taking from the disk cache.
        /// </summary>
        /// <value>The download cache.</value>
        IDownloadCache DownloadCache { get; set; }

        /// <summary>
        /// Gets a value indicating whether ImageService will pause tasks execution
        /// </summary>
        /// <value><c>true</c> if pause work; otherwise, <c>false</c>.</value>
        bool PauseWork { get; }

		/// <summary>
		/// Sets a value indicating if all loading work should be paused.
		/// </summary>
		/// <param name="pauseWork">If set to <c>true</c> pause work.</param>
		/// <param name="cancelExisting">If set to <c>true</c> cancels existing tasks.</param>
		void SetPauseWork(bool pauseWork, bool cancelExisting = false);

		/// <summary>
		/// Sets a value indicating if all loading work should be paused. Also cancels existing tasks.
		/// </summary>
		/// <param name="pauseWork">If set to <c>true</c> pause work.</param>
		void SetPauseWorkAndCancelExisting(bool pauseWork);

		/// <summary>
		/// Cancel any loading work for the given task
		/// </summary>
		/// <param name="task">Image loading task to cancel.</param>
		void CancelWorkFor(IImageLoaderTask task);

        /// <summary>
        /// Removes a pending image loading task from the work queue.
        /// </summary>
        /// <param name="task">Image loading task to remove.</param>
        void RemovePendingTask(IImageLoaderTask task);

        /// <summary>
        /// Queue an image loading task.
        /// </summary>
        /// <param name="imageSource">image source to load</param>
        /// <param name="decoder">renderer supplied decoder; used for bitmap images</param>
        void LoadImage(ILoadableImageSource imageSource, ImageDecoder? decoder);

        /// <summary>
        /// Invalidates selected caches.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        /// <param name="cacheType">Memory cache, Disk cache or both</param>
        Task InvalidateCacheAsync(CacheType cacheType);

        /// <summary>
        /// Invalidates the memory cache.
        /// </summary>
        void InvalidateMemoryCache();

        /// <summary>
        /// Invalidates the disk cache.
        /// </summary>
        Task InvalidateDiskCacheAsync();

        /// <summary>
        /// Invalidates the cache for given key.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Concerns images with this key.</param>
        /// <param name="cacheType">Memory cache, Disk cache or both</param>
        /// <param name="removeSimilar">If similar keys should be removed, ie: typically keys with extra transformations</param>
        Task InvalidateCacheEntryAsync(string key, CacheType cacheType, bool removeSimilar=false);

        /// <summary>
        /// Cancels tasks that match predicate.
        /// </summary>
        /// <param name="predicate">Predicate for finding relevant tasks to cancel.</param>
        void Cancel(Func<IImageLoaderTask, bool> predicate);
    }
}
