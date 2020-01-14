using XGraphics.StandardModel;

namespace XGraphics.ImageLoading
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
