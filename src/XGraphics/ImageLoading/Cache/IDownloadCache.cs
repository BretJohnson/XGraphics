using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Work;

namespace XGraphics.ImageLoading.Cache
{
    public interface IDownloadCache
    {
        Task<Stream> DownloadAndCacheIfNeededAsync(string url, ILoadableImageSource imageSource, CancellationToken token);
    }
}
