using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading;
using XGraphics.ImageLoading.Work;

namespace XGraphics
{
    public class UriSource : DataSource
    {
        public UriSource(Uri uri)
        {
            Uri = uri;
        }

        Uri Uri { get; }

        public override async Task<Stream> ResolveAsync(ILoadableImageSource imageSource, IImageLoader imageLoader,
            CancellationToken token)
        {
            string identifier = Uri.ToString();
            Stream stream = await imageLoader.DownloadCache.DownloadAndCacheIfNeededAsync(identifier, imageSource, token).ConfigureAwait(false);

            if (token.IsCancellationRequested)
            {
                stream.TryDispose();
                token.ThrowIfCancellationRequested();
            }

            return stream;
        }

        public override string? Key => Uri.ToString();
    }
}
