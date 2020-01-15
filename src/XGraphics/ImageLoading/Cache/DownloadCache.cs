using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Helpers;

namespace XGraphics.ImageLoading.Cache
{
    public class DownloadCache : IDownloadCache
    {
        private readonly ImageLoader _imageLoader;

        public DownloadCache(ImageLoader imageLoader)
        {
            _imageLoader = imageLoader;
        }

        public string[] InvalidContentTypes { get; set; } = new[] { "text/html", "application/json", "audio/", "video/", "message" };

        public virtual async Task<Stream> DownloadAndCacheIfNeededAsync(string url, ILoadableImageSource imageSource, CancellationToken token)
        {
            string filename = MD5Helper.MD5(url);
            bool allowDiskCaching = AllowDiskCaching(_imageLoader.CacheType);
            IDiskCache? diskCache = _imageLoader.DiskCache;
            TimeSpan duration = _imageLoader.DiskCacheDuration;

            if (allowDiskCaching)
            {
                Stream? diskStream = await diskCache!.TryGetStreamAsync(filename).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();

                if (diskStream != null)
                {
                    return diskStream;
                }
            }

            var downloadInfo = new DownloadInformation(duration);
#if LATER
            parameters.OnDownloadStarted?.Invoke(downloadInfo);
#endif

            var responseBytes = await Retry.DoAsync(
                async () => await DownloadAsync(url, token, _imageLoader.HttpClient, imageSource, downloadInfo).ConfigureAwait(false),
                TimeSpan.FromMilliseconds(_imageLoader.RetryDelayInMs),
                _imageLoader.RetryCount,
                () => _imageLoader.Logger?.Debug($"Retry download: {url}")).ConfigureAwait(false);

            if (responseBytes == null)
                throw new HttpRequestException("No Content");

            if (allowDiskCaching)
            {
                await diskCache!.AddToSavingQueueIfNotExistsAsync(filename, responseBytes, downloadInfo.CacheValidity).ConfigureAwait(false);
            }

            token.ThrowIfCancellationRequested();

            return new MemoryStream(responseBytes, false);
        }
        
        protected virtual async Task<byte[]> DownloadAsync(string url, CancellationToken token, HttpClient client, ILoadableImageSource imageSource, DownloadInformation downloadInfo)
        {
            await Task.Delay(25, token).ConfigureAwait(false);
            token.ThrowIfCancellationRequested();

            using (var httpHeadersTimeoutTokenSource = new CancellationTokenSource())
            using (var headersTimeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, httpHeadersTimeoutTokenSource.Token))
            {
                httpHeadersTimeoutTokenSource.CancelAfter(TimeSpan.FromSeconds(_imageLoader.HttpHeadersTimeout));

                try
                {
                    CancellationToken headerTimeoutToken = headersTimeoutTokenSource.Token;

                    using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, headerTimeoutToken).ConfigureAwait(false))
                    {
                        headerTimeoutToken.ThrowIfCancellationRequested();

                        if (!response.IsSuccessStatusCode)
                        {
                            if (response.Content == null)
                                throw new DownloadHttpStatusCodeException(response.StatusCode);

                            using (response.Content)
                            {
                                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                throw new DownloadHttpStatusCodeException(response.StatusCode, content);
                            }
                        }

                        if (response.Content == null)
                            throw new DownloadException("No content");

                        string? mediaType = response.Content.Headers?.ContentType?.MediaType;
                        if (!string.IsNullOrWhiteSpace(mediaType) && !mediaType!.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                        {
                            if (InvalidContentTypes.Any(v => mediaType.StartsWith(v, StringComparison.OrdinalIgnoreCase)))
                                throw new DownloadException($"Invalid response content type ({mediaType})");
                        }

                        if (_imageLoader.TryToReadDiskCacheDurationFromHttpHeaders
                            && response.Headers?.CacheControl?.MaxAge != null && response.Headers.CacheControl.MaxAge > TimeSpan.Zero)
                        {
                            downloadInfo.CacheValidity = response.Headers.CacheControl.MaxAge.Value;
                        }

                        using (var httpReadTimeoutTokenSource = new CancellationTokenSource())
                        using (var readTimeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, httpReadTimeoutTokenSource.Token))
                        {
                            CancellationToken readTimeoutToken = readTimeoutTokenSource.Token;
                            CancellationToken httpReadTimeoutToken = httpReadTimeoutTokenSource.Token;
                            int total = (int)(response.Content.Headers?.ContentLength ?? -1);

                            httpReadTimeoutTokenSource.CancelAfter(_imageLoader.HttpClient.Timeout);
                            readTimeoutToken.ThrowIfCancellationRequested();

                            try
                            {
                                using (var outputStream = new MemoryStream())
                                using (var sourceStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                                {
                                    httpReadTimeoutToken.Register(() => sourceStream.TryDispose());

                                    int totalRead = 0;
                                    byte[] buffer = new byte[_imageLoader.HttpReadBufferSize];
                                    int read = 0;
                                    int currentProgress = 1;

#if LATER
                                    imageSource.RaiseDownloadProgress(new DownloadProgressEventArgs(currentProgress));
#endif

                                    while ((read = await sourceStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                                    {
                                        readTimeoutToken.ThrowIfCancellationRequested();
                                        outputStream.Write(buffer, 0, read);
                                        totalRead += read;

                                        if (total > 0)
                                            currentProgress = totalRead * 100 / total;
                                        else currentProgress = ++currentProgress;

                                        if (currentProgress < 1)
                                            currentProgress = 1;
                                        if (currentProgress > 99)
                                            currentProgress = 99;

#if LATER
                                        imageSource.RaiseDownloadProgress(new DownloadProgressEventArgs(currentProgress));
#endif
                                    }

                                    if (outputStream.Length == 0)
                                        throw new DownloadException("Zero length stream");

                                    if (outputStream.Length < 32)
                                        throw new DownloadException("Invalid stream");

                                    currentProgress = 100;
#if LATER
                                    imageSource.RaiseDownloadProgress(new DownloadProgressEventArgs(currentProgress));
#endif

                                    return outputStream.ToArray();
                                }
                            }
                            catch (Exception ex) when (ex is OperationCanceledException || ex is ObjectDisposedException)
                            {
                                if (httpReadTimeoutTokenSource.IsCancellationRequested)
                                    throw new DownloadReadTimeoutException();

                                throw;
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    if (httpHeadersTimeoutTokenSource.IsCancellationRequested)
                        throw new DownloadHeadersTimeoutException();
                    else
                        throw;
                }
            }
        }

        protected virtual bool AllowDiskCaching(CacheType? cacheType)
        {
            return cacheType.HasValue == false || cacheType == CacheType.All || cacheType == CacheType.Disk;
        }
    }
}
