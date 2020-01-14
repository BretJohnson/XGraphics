using Android.Content;
using XGraphics.ImageLoading.Work;

namespace XGraphics.SkiaRenderer.Android
{
    public sealed class AndroidSkiaXGraphicsRenderer : SkiaXGraphicsRenderer
    {
        public AndroidSkiaXGraphicsRenderer()
        {
            ImageLoader = new AndroidImageLoader();
        }

        public override IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            return new AndroidXGraphicsView((Context) arg1, this);
        }

        public override IImageLoader ImageLoader { get; set; }
    }
}
