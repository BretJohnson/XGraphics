using System;
using XGraphics.ImageLoading.Work;

namespace XGraphics.ImageLoading
{
    /// <summary>
    /// Configuration.
    /// </summary>
    public class ImageLoaderConfiguration
    {
        public static ImageLoaderConfiguration Default { get; } = new ImageLoaderConfiguration();

        public ImageLoaderConfiguration()
        {
            // default values here:
            BitmapOptimizations = true;
            StreamChecksumsAsKeys = true;
            AnimateGifs = true;
            InvalidateLayout = true;
        }

#if OLD
        /// <summary>
        /// Gets or sets the scheduler used to organize/schedule image loading tasks.
        /// </summary>
        /// <value>The scheduler used to organize/schedule image loading tasks.</value>
        public IWorkScheduler? Scheduler { get; set; }

        /// <summary>
        /// Gets or sets the logger used to receive debug/error messages.
        /// </summary>
        /// <value>The logger.</value>
        public IMiniLogger? Logger { get; set; }

        /// <summary>
        /// Gets or sets the disk cache.
        /// </summary>
        /// <value>The disk cache.</value>
        public IDiskCache? DiskCache { get; set; }

        /// <summary>
        /// Gets or sets the disk cache path.
        /// </summary>
        /// <value>The disk cache path.</value>
        public string? DiskCachePath { get; set; }

        /// <summary>
        /// Gets or sets the download cache. Download cache is responsible for retrieving data from the web, or taking from the disk cache.
        /// </summary>
        /// <value>The download cache.</value>
        public IDownloadCache? DownloadCache { get; set; }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:XGraphics.Configuration"/> bitmap
        /// memory optimizations.
        /// </summary>
        /// <value><c>true</c> if bitmap memory optimizations; otherwise, <c>false</c>.</value>
        public bool BitmapOptimizations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:XGraphics.Configuration"/> stream
        /// checksums as keys.
        /// </summary>
        /// <value><c>true</c> if stream checksums as keys; otherwise, <c>false</c>.</value>
        public bool StreamChecksumsAsKeys { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether upscaling should be used in <see cref="TaskParameter.DownSample"/>/<see cref="TaskParameter.DownSampleInDip"/> functions if the image is smaller than passed dimensions
        /// </summary>
        /// <value><c>true</c> if upscaling is allowed; otherwise, <c>false</c></value>
        public bool AllowUpscale { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the memory cache in bytes.
        /// </summary>
        /// <value>The maximum size of the memory cache in bytes.</value>
        public int MaxMemoryCacheSize { get; set; }

#if false
        /// <summary>
        /// Milliseconds to wait prior to start any task.
        /// </summary>
        public int DelayInMs { get; set; }

        /// <summary>
        /// Enables / disables verbose performance logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose performance logging.</value>
        public bool VerbosePerformanceLogging { get; set; }

        /// <summary>
        /// Enables / disables verbose memory cache logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose memory cache logging.</value>
        public bool VerboseMemoryCacheLogging { get; set; }

        /// <summary>
        /// Enables / disables verbose image loading cancelled logging.
        /// WARNING! It will downgrade image loading performance, disable it for release builds!
        /// </summary>
        /// <value>The verbose image loading cancelled logging.</value>
        public bool VerboseLoadingCancelledLogging { get; set; }

        /// <summary>
        /// Enables / disables  verbose logging.
        /// IMPORTANT! If it's disabled are other verbose logging options are disabled too
        /// </summary>
        /// <value>The verbose logging.</value>
        public bool VerboseLogging { get; set; }

        /// <summary>
        /// Gets or sets the scheduler max parallel tasks.
        /// Default: Environment.ProcessorCount <= 2 ? 1 : 2;
        /// </summary>
        /// <value>The decoding max parallel tasks.</value>
        public int DecodingMaxParallelTasks { get; set; }

        /// <summary>
        /// Gets or sets the scheduler max parallel tasks.
        /// Default: Math.Max(16, 4 + Environment.ProcessorCount);
        /// </summary>
        /// <value>The scheduler max parallel tasks.</value>
        public int SchedulerMaxParallelTasks { get; set; }
#endif

        /// <summary>
        /// Gets or sets the scheduler max parallel tasks factory.
        /// If null SchedulerMaxParallelTasks property is used
        /// </summary>
        /// <value>The scheduler max parallel tasks factory.</value>
        public Func<ImageLoaderConfiguration, int>? SchedulerMaxParallelTasksFactory { get; set; }

#if OLD
        /// <summary>
        /// Gets or sets the default duration of the disk cache entries.
        /// </summary>
        /// <value>The duration of the cache.</value>
        public TimeSpan DiskCacheDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether try to read
        /// disk cache duration from http headers .
        /// </summary>
        /// <value><c>true</c> if try to read disk cache duration from http headers; otherwise, <c>false</c>.</value>
        public bool TryToReadDiskCacheDurationFromHttpHeaders { get; set; }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether image loader should animate gifs.
        /// </summary>
        /// <value><c>true</c> if animate gifs; otherwise, <c>false</c>.</value>
        public bool AnimateGifs { get; set; }

#if false
        /// <summary>
        /// Gets or sets a value indicating whether clear
        /// memory cache on out of memory.
        /// </summary>
        /// <value><c>true</c> if clear memory cache on out of memory; otherwise, <c>false</c>.</value>
        public bool ClearMemoryCacheOnOutOfMemory { get; set; }
#endif

        /// <summary>
        /// Specifies if view layout should be invalidated after image is loaded
        /// </summary>
        /// <value><c>true</c> if invalidate layout; otherwise, <c>false</c>.</value>
        public bool InvalidateLayout { get; set; }
    }
}

