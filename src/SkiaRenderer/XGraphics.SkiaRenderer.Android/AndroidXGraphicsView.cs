using Android.Content;
using SkiaSharp;
using XGraphics.ImageLoading;
using XGraphics.ImageLoading.Work;

namespace XGraphics.SkiaRenderer.Android
{
    /// <summary>
    /// This code was adapted from https://github.com/mono/SkiaSharp/blob/e64780c8c8e313ef518a56c6dec2b890ff2877a5/source/SkiaSharp.Views/SkiaSharp.Views.Android/SKGLTextureView.cs
    /// </summary>
    public class AndroidXGraphicsView : GLTextureView, IXGraphicsView
    {
        private readonly SKGLTextureViewRenderer _renderer;

        public IXCanvas? Content { get; set; } = null;

        public AndroidXGraphicsView(Context context, AndroidSkiaXGraphicsRenderer xGraphicsRenderer)
            : base(context)
        {
            XGraphicsRenderer = xGraphicsRenderer;

            SetEGLContextClientVersion(2);
            SetEGLConfigChooser(8, 8, 8, 8, 0, 8);

            _renderer = new InternalRenderer(this);
            SetRenderer(_renderer);
        }

        public SKSize CanvasSize => _renderer.CanvasSize;

        public GRContext GRContext => _renderer.GRContext;

        public object? NativeControl => this;

        public AndroidSkiaXGraphicsRenderer XGraphicsRenderer { get; }

        private class InternalRenderer : SKGLTextureViewRenderer
        {
            private readonly AndroidXGraphicsView _view;

            public InternalRenderer(AndroidXGraphicsView view)
            {
                _view = view;
            }

            protected override void PaintSurface(SKSurface skSurface, GRBackendRenderTarget grBackendRenderTarget, GRSurfaceOrigin grSurfaceOrigin, 
                SKColorType skColorType)
            {
                // Paint all elements from the canvas on the surface
                new SkiaPainter(skSurface, _view.XGraphicsRenderer.ImageLoader).Paint(_view.Content);
            }
        }
    }
}
