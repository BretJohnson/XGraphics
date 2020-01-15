using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace XGraphics.ImageLoading.Cache
{
    public interface IDownloadCache
    {
        Task<Stream> DownloadAndCacheIfNeededAsync(string url, ILoadableImageSource imageSource, CancellationToken token);
    }
}
