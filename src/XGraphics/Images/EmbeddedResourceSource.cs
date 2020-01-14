using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading.Work;

namespace XGraphics
{
    public class EmbeddedResourceSource : DataSource
    {
        public override Task<Stream> ResolveAsync(ILoadableImageSource imageSource, IImageLoader imageLoader,
            CancellationToken token) => throw new System.NotImplementedException();

        public override string? Key => throw new System.NotImplementedException();
    }
}
