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

        private GRContext context;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;

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

        public SKSize CanvasSize => renderTarget.Size;

        public GRContext GRContext => context;

        public new void DrawInRect(GLKView view, CGRect rect)
        {
#if false
            if (designMode)
                return;
#endif

            // create the contexts if not done already
            if (context == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();
                context = GRContext.Create(GRBackend.OpenGL, glInterface);
            }

            // manage the drawing surface
            if (renderTarget == null || surface == null || renderTarget.Width != DrawableWidth || renderTarget.Height != DrawableHeight)
            {
                // create or update the dimensions
                renderTarget?.Dispose();
                Gles.glGetIntegerv(Gles.GL_FRAMEBUFFER_BINDING, out var framebuffer);
                Gles.glGetIntegerv(Gles.GL_STENCIL_BITS, out var stencil);
                var glInfo = new GRGlFramebufferInfo((uint)framebuffer, colorType.ToGlSizedFormat());
                renderTarget = new GRBackendRenderTarget((int)DrawableWidth, (int)DrawableHeight, context.GetMaxSurfaceSampleCount(colorType), stencil, glInfo);

                // create the surface
                surface?.Dispose();
                surface = SKSurface.Create(context, renderTarget, surfaceOrigin, colorType);
            }

            using (new SKAutoCanvasRestore(surface.Canvas, true))
            {
                // start drawing
                //var e = new SKPaintGLSurfaceEventArgs(surface, renderTarget, surfaceOrigin, colorType);
                PaintSurface(surface);
            }

            // flush the SkiaSharp contents to GL
            surface.Canvas.Flush();
            context.Flush();
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