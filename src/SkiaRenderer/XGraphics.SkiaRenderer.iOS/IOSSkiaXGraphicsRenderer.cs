using XGraphics.ImageLoading.Work;

namespace XGraphics.SkiaRenderer.iOS
{
    public sealed class IOSSkiaXGraphicsRenderer : SkiaXGraphicsRenderer
    {
        public IOSSkiaXGraphicsRenderer()
        {
            ImageLoader = new IOSImageLoader();
        }

        public override IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            return new IOSXGraphicsView(this);
        }

        public override IImageLoader ImageLoader { get; set; }
    }
}
