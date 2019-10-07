using Android.Content;

namespace XGraphics.Renderer.Skia.Android
{
    public class AndroidSkiaXGraphicsRenderer : SkiaXGraphicsRenderer
    {
        public override IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            return new AndroidXGraphicsView((Context) arg1);
        }
    }
}