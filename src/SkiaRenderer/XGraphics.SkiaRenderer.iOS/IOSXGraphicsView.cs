using SkiaSharp;

namespace XGraphics.SkiaRenderer.iOS
{
    public class IOSXGraphicsView : SKGLView, IXGraphicsView
    {
        public IXCanvas? Content { get; set; } = null;

        public object? NativeControl => this;

        protected override void PaintSurface(SKSurface surface)
        {
            // Paint all elements from the canvas on the surface
            new SkiaPainter(surface, new IOSImageProvider()).Paint(Content);
        }
    }
}
