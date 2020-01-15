using XGraphics.ImageLoading;
using XGraphics.StandardModel;

namespace XGraphics
{
    public class VectorLoadedImage : LoadedImage
    {
        public VectorLoadedImage(XCanvas canvas)
        {
            Canvas = canvas;
        }

        public XCanvas Canvas { get; }
    }
}
