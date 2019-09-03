namespace XGraphics.Renderer.Skia.iOS
{
    public class IOSSkiaXGraphicsRenderer : SkiaXGraphicsRenderer
    {
        public override IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            return new IOSXGraphicsView();
        }
    }
}