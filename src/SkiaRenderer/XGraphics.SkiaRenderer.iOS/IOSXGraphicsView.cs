using SkiaSharp;

namespace XGraphics.SkiaRenderer.iOS
{
    public class IOSXGraphicsView : SKGLView, IXGraphicsView
    {
        public IOSXGraphicsView(IOSSkiaXGraphicsRenderer xGraphicsRenderer)
        {
            XGraphicsRenderer = xGraphicsRenderer;
        }

        public IXCanvas? Content { get; set; } = null;

        public object? NativeControl => this;

        public IOSSkiaXGraphicsRenderer XGraphicsRenderer { get; }

        protected override void PaintSurface(SKSurface surface)
        {
            // Paint all elements from the canvas on the surface
            new SkiaPainter(surface, XGraphicsRenderer.ImageLoader).Paint(Content);
        }
    }
}
