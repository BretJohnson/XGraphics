using SkiaSharp;

namespace XGraphics.Renderer.Skia.iOS
{
    public class IOSXGraphicsView : SKGLView, IXGraphicsView
    {
        public IXGraphics? Content { get; set; } = null;

        public object? NativeControl => this;

        protected override void PaintSurface(SKSurface surface)
        {
            // Paint all elements from the canvas on the surface
            new SkiaPainter(surface).Paint(Content);
        }
    }
}