using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XGraphics.ImageLoading;

namespace XGraphics
{
    public abstract class ImageDecoder
    {
        public abstract Task<LoadedImage> DecodeAsync(Stream stream, CancellationToken cancellationToken);
    }
}
