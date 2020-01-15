using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Cache;
using XGraphics.ImageLoading.Extensions;
using XGraphics.ImageLoading.Helpers;

namespace XGraphics.ImageLoading.Work
{
    public class ImageLoaderTask : IImageLoaderTask
    {
        private readonly ImageLoader _imageLoader;
        private readonly ImageDecoder? _decoder;
        private readonly IMemoryCache? _memoryCache;

        public ImageLoaderTask(ImageLoader imageLoader, ILoadableImageSource imageSource, ImageDecoder? decoder)
        {
            _imageLoader = imageLoader;
            ImageSource = imageSource;
            _decoder = decoder;
            _memoryCache = _imageLoader.MemoryCache;

			CancellationTokenSource = new CancellationTokenSource();

            string? sourceKey = imageSource.Source.Key;
            if (sourceKey != null)
            {
                KeyRaw = sourceKey;
            }
            else
            {
                // If there's no key then we can't use the memory cache
                _memoryCache = null;
                KeyRaw = string.Concat("Stream_", Guid.NewGuid().ToString("N"));
            }

            if (string.IsNullOrWhiteSpace(KeyRaw))
                throw new Exception("Key cannot be empty");

            if (imageSource is IBitmapImageSource bitmapImageSource && (bitmapImageSource.DecodePixelWidth > 0 || bitmapImageSource.DecodePixelHeight > 0))
            {
                KeyDownsamplingOnly = $";{bitmapImageSource.DecodePixelWidth}x{bitmapImageSource.DecodePixelHeight}";
            }
            else
            {
                KeyDownsamplingOnly = string.Empty;
            }

            Key = string.Concat(KeyRaw, KeyDownsamplingOnly);
		}

        public ILoadableImageSource ImageSource { get; }

        public int? Priority { get; set; } = null;

        public bool CanUseMemoryCache => _memoryCache != null;

        protected CancellationTokenSource CancellationTokenSource { get; }

		public bool IsCancelled
		{
			get
			{
				try
				{
					return _isDisposed || (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested);
				}
				catch (ObjectDisposedException)
				{
					return true;
				}
			}
		}

		public bool IsCompleted { get; private set; }

		public string Key { get; }

		public string KeyRaw { get; }

		protected string KeyDownsamplingOnly { get; }

		protected void ThrowIfCancellationRequested()
		{
			try
			{
				CancellationTokenSource?.Token.ThrowIfCancellationRequested();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		public void Cancel()
		{
			if (!_isDisposed)
			{
				if (IsCancelled || IsCompleted)
					return;

				_imageLoader.RemovePendingTask(this);

				try
				{
					CancellationTokenSource?.Cancel();
				}
				catch (ObjectDisposedException)
				{
				}

				if (_imageLoader.VerboseLoadingCancelledLogging)
					_imageLoader.Logger?.Debug($"Image loading cancelled: {Key}");
			}
		}

		protected virtual async Task<LoadedImage> GenerateImageAsync(ILoadableImageSource source, Stream imageStream)
		{
            LoadedImage loadedImage;
            if (_decoder != null)
            {
                loadedImage = await _decoder.DecodeAsync(imageStream, CancellationTokenSource.Token);
            }
            else if (source.Decoder != null)
            {
                loadedImage = await source.Decoder.DecodeAsync(imageStream, CancellationTokenSource.Token);
            }
            else throw new Exception($"Decoder must be supplied for image format, as it's not built in: {ImageSource.GetType()}");

            // TODO: Maybe do this
#if false
            if (ex is Java.Lang.Throwable javaException && javaException.Class == Java.Lang.Class.FromType(typeof(Java.Lang.OutOfMemoryError)))
            {
                if (Configuration.ClearMemoryCacheOnOutOfMemory)
                    Java.Lang.JavaSystem.Gc();

                throw new OutOfMemoryException();
            }
#endif
            return loadedImage;
		}

		public virtual bool TryLoadFromMemoryCache()
        {
            if (_memoryCache == null)
                return false;

            try
			{
                LoadedImage found = _memoryCache.Get(Key);
                if (found == null)
                    return false;

#if LATER
                ImageSource.LoadSucceeded(found);
#endif

                _imageLoader.Logger?.Debug($"Image loaded from cache: {Key}");
                IsCompleted = true;

                return true;
			}
			catch (Exception ex)
			{
				if (_imageLoader.ClearMemoryCacheOnOutOfMemory && ex is OutOfMemoryException)
				{
					_memoryCache.Clear();
				}

                _imageLoader.Logger?.Error($"Image loading from memory cached failed: {Key}", ex);

                return false;
			}
        }

        public async Task RunAsync()
        {
            IMiniLogger? logger = _imageLoader.Logger;

            try
			{
				// LOAD IMAGE
				if (! TryLoadFromMemoryCache())
				{
					if (_imageLoader.DelayInMs > 0)
					{
						await Task.Delay(_imageLoader.DelayInMs).ConfigureAwait(false);
					}

					logger?.Debug($"Generating/retrieving image: {Key}");
                    Stream resolvedStream = await ImageSource.Source.ResolveAsync(ImageSource, _imageLoader, CancellationTokenSource.Token).ConfigureAwait(false);

                    LoadedImage decodedImage;
					using (resolvedStream)
					{
                        ThrowIfCancellationRequested();

                        decodedImage = await GenerateImageAsync(ImageSource, resolvedStream).ConfigureAwait(false);
					}

					ThrowIfCancellationRequested();

                    _memoryCache?.Add(Key, decodedImage);

                    ThrowIfCancellationRequested();

#if LATER
                    ImageSource.LoadSucceeded(decodedImage);
#endif
				}
			}
			catch (Exception ex)
			{
				if (ex is OperationCanceledException || ex is ObjectDisposedException)
				{
					if (_imageLoader.VerboseLoadingCancelledLogging)
					{
                        logger?.Debug($"Image loading cancelled: {Key}");
					}
				}
				else
				{
					if (_imageLoader.ClearMemoryCacheOnOutOfMemory && ex is OutOfMemoryException)
					{
						_imageLoader.MemoryCache?.Clear();
					}

                    logger?.Error($"Image loading failed: {Key}", ex);
#if LATER
                    ImageSource.LoadFailed(ex);
#endif
				}
			}
			finally
			{
				try
				{
					if (CancellationTokenSource?.IsCancellationRequested == false)
						CancellationTokenSource.Cancel();
				}
				catch { }

				IsCompleted = true;

				_imageLoader.RemovePendingTask(this);
			}
		}

		private bool _isDisposed;

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;

				CancellationTokenSource.TryDispose();
			}
		}
	}
}
