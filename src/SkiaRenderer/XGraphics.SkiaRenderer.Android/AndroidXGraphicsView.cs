using Android.Content;
using SkiaSharp;

namespace XGraphics.SkiaRenderer.Android
{
    /// <summary>
    /// This code was adapted from https://github.com/mono/SkiaSharp/blob/e64780c8c8e313ef518a56c6dec2b890ff2877a5/source/SkiaSharp.Views/SkiaSharp.Views.Android/SKGLTextureView.cs
    /// </summary>
    public class AndroidXGraphicsView : GLTextureView, IXGraphicsView
    {
        private readonly SKGLTextureViewRenderer renderer;

        public IXCanvas? Content { get; set; } = null;

        public AndroidXGraphicsView(Context context)
            : base(context)
        {
            SetEGLContextClientVersion(2);
            SetEGLConfigChooser(8, 8, 8, 8, 0, 8);

            renderer = new InternalRenderer(this);
            SetRenderer(renderer);
        }

        public SKSize CanvasSize => renderer.CanvasSize;

        public GRContext GRContext => renderer.GRContext;

        public object? NativeControl => this;

        private class InternalRenderer : SKGLTextureViewRenderer
        {
            private readonly AndroidXGraphicsView _view;

            public InternalRenderer(AndroidXGraphicsView view)
            {
                this._view = view;
            }

            protected override void PaintSurface(SKSurface skSurface, GRBackendRenderTarget grBackendRenderTarget, GRSurfaceOrigin grSurfaceOrigin, 
                SKColorType skColorType)
            {
                // Paint all elements from the canvas on the surface
                new SkiaPainter(skSurface).Paint(_view.Content);
            }
        }
    }
}
