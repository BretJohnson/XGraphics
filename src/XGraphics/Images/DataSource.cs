using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Work;

namespace XGraphics
{
    public abstract class DataSource
    {
        public abstract Task<Stream> ResolveAsync(ILoadableImageSource imageSource, IImageLoader imageLoader,
            CancellationToken token);

        public virtual int DefaultLoadingPriority => (int) LoadingPriority.Normal;

        /// <summary>
        /// The Key is used for caching. If null, there is no caching.
        /// </summary>
        public abstract string? Key { get; }
    }
}
