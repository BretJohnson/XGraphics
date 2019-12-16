using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using GLKit;
using OpenGLES;
using SkiaSharp;
#if !__WATCHOS__
using XGraphics.SkiaRenderer.iOS.GlesInterop;

#if __TVOS__
namespace XGraphics.SkiaRenderer.tvOS
#elif __IOS__
namespace XGraphics.SkiaRenderer.iOS
#endif
{
    ///
    /// This code was adapted form https://github.com/mono/SkiaSharp/blob/d5aa34a78786a9e24c13655694760bf3ee4db45a/source/SkiaSharp.Views/SkiaSharp.Views.AppleiOS/SKGLView.cs
    /// 
    [Register(nameof(SKGLView))]
    [DesignTimeVisible(true)]
    public abstract class SKGLView : GLKView, IGLKViewDelegate, IComponent
    {
        private const SKColorType colorType = SKColorType.Rgba8888;
        private const GRSurfaceOrigin surfaceOrigin = GRSurfaceOrigin.BottomLeft;

        // for IComponent
#pragma warning disable 67
        private event EventHandler DisposedInternal;
#pragma warning restore 67
        ISite IComponent.Site { get; set; }
        event EventHandler IComponent.Disposed {
            add { DisposedInternal += value; }
            remove { DisposedInternal -= value; }
        }

        //private bool designMode;

        private GRContext _context;
        private GRBackendRenderTarget _renderTarget;
        private SKSurface _surface;

        // created in code
        public SKGLView()
        {
            Initialize();
        }

        // created in code
        public SKGLView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        // created via designer
        public SKGLView(IntPtr p)
            : base(p)
        {
        }

        // created via designer
        public override void AwakeFromNib()
        {
            Initialize();
        }

        private void Initialize()
        {
#if false
            designMode = ((IComponent)this).Site?.DesignMode == true || !Extensions.IsValidEnvironment;

            if (designMode)
                return;
#endif

            // create the GL context
            Context = new EAGLContext(EAGLRenderingAPI.OpenGLES2);
            DrawableColorFormat = GLKViewDrawableColorFormat.RGBA8888;
            DrawableDepthFormat = GLKViewDrawableDepthFormat.Format24;
            DrawableStencilFormat = GLKViewDrawableStencilFormat.Format8;
            DrawableMultisample = GLKViewDrawableMultisample.Sample4x;

            // hook up the drawing 
            Delegate = this;
        }

        public SKSize CanvasSize => _renderTarget.Size;

        public GRContext GRContext => _context;

        public new void DrawInRect(GLKView view, CGRect rect)
        {
#if false
            if (designMode)
                return;
#endif

            // create the contexts if not done already
            if (_context == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();
                _context = GRContext.Create(GRBackend.OpenGL, glInterface);
            }

            // manage the drawing surface
            if (_renderTarget == null || _surface == null || _renderTarget.Width != DrawableWidth || _renderTarget.Height != DrawableHeight)
            {
                // create or update the dimensions
                _renderTarget?.Dispose();
                Gles.glGetIntegerv(Gles.GL_FRAMEBUFFER_BINDING, out var framebuffer);
                Gles.glGetIntegerv(Gles.GL_STENCIL_BITS, out var stencil);
                var glInfo = new GRGlFramebufferInfo((uint)framebuffer, colorType.ToGlSizedFormat());
                _renderTarget = new GRBackendRenderTarget((int)DrawableWidth, (int)DrawableHeight, _context.GetMaxSurfaceSampleCount(colorType), stencil, glInfo);

                // create the surface
                _surface?.Dispose();
                _surface = SKSurface.Create(_context, _renderTarget, surfaceOrigin, colorType);
            }

            using (new SKAutoCanvasRestore(_surface.Canvas, true))
            {
                // start drawing
                //var e = new SKPaintGLSurfaceEventArgs(surface, renderTarget, surfaceOrigin, colorType);
                PaintSurface(_surface);
            }

            // flush the SkiaSharp contents to GL
            _surface.Canvas.Flush();
            _context.Flush();
        }

        protected abstract void PaintSurface(SKSurface surface);

        public override CGRect Frame {
            get => base.Frame;
            set {
                base.Frame = value;
                SetNeedsDisplay();
            }
        }
    }
}

#endif
