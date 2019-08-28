using System;
using Android.Opengl;
using Javax.Microedition.Khronos.Opengles;
using SkiaSharp;
using EGLConfig = Javax.Microedition.Khronos.Egl.EGLConfig;

namespace XGraphics.Renderer.Skia.Android
{
    /// <summary>
    /// This code was copied from https://github.com/mono/SkiaSharp/blob/9665c11f45b0444e2a44976b3053b07df21a60b3/source/SkiaSharp.Views/SkiaSharp.Views.Android/SKGLTextureViewRenderer.cs
    /// </summary>
    public abstract class SKGLTextureViewRenderer : Java.Lang.Object, GLTextureView.IRenderer
    {
        private const SKColorType colorType = SKColorType.Rgba8888;
        private const GRSurfaceOrigin surfaceOrigin = GRSurfaceOrigin.BottomLeft;

        private GRContext? context;
        private GRBackendRenderTarget? renderTarget;
        private SKSurface? surface;
        private int surfaceWidth;
        private int surfaceHeight;

        public SKSize CanvasSize => renderTarget!.Size;

        public GRContext GRContext => context;

        protected abstract void PaintSurface(SKSurface skSurface, GRBackendRenderTarget grBackendRenderTarget,
            GRSurfaceOrigin grSurfaceOrigin, SKColorType skColorType);

        public void OnDrawFrame(IGL10 gl)
        {
            GLES10.GlClear(GLES10.GlColorBufferBit | GLES10.GlDepthBufferBit | GLES10.GlStencilBufferBit);

            // create the contexts if not done already
            if (context == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();
                context = GRContext.Create(GRBackend.OpenGL, glInterface);
            }

            // manage the drawing surface
            if (renderTarget == null || surface == null || renderTarget.Width != surfaceWidth || renderTarget.Height != surfaceHeight)
            {
                // create or update the dimensions
                renderTarget?.Dispose();
                var buffer = new int[2];
                GLES20.GlGetIntegerv(GLES20.GlFramebufferBinding, buffer, 0);
                GLES20.GlGetIntegerv(GLES20.GlStencilBits, buffer, 1);
                var glInfo = new GRGlFramebufferInfo((uint)buffer[0], colorType.ToGlSizedFormat());
                renderTarget = new GRBackendRenderTarget(surfaceWidth, surfaceHeight, context.GetMaxSurfaceSampleCount(colorType), buffer[1], glInfo);

                // create the surface
                surface?.Dispose();
                surface = SKSurface.Create(context, renderTarget, surfaceOrigin, colorType);
            }

            using (new SKAutoCanvasRestore(surface.Canvas, true))
            {
                // start drawing
                PaintSurface(surface, renderTarget, surfaceOrigin, colorType);
            }

            // flush the SkiaSharp contents to GL
            surface.Canvas.Flush();
            context.Flush();
        }

        public void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            GLES20.GlViewport(0, 0, width, height);

            surfaceWidth = width;
            surfaceHeight = height;
        }

        public void OnSurfaceCreated(IGL10 gl, EGLConfig config)
        {
            FreeContext();
        }

        public void OnSurfaceDestroyed()
        {
            FreeContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FreeContext();
            }
            base.Dispose(disposing);
        }

        private void FreeContext()
        {
            surface?.Dispose();
            surface = null;
            renderTarget?.Dispose();
            renderTarget = null;
            context?.Dispose();
            context = null;
        }
    }
}
