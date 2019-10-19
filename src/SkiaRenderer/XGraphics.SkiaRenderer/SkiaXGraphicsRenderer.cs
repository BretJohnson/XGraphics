﻿using System;
using SkiaSharp;

namespace XGraphics.SkiaRenderer
{
    public class SkiaXGraphicsRenderer : XGraphicsRenderer
    {
        public override void RenderToBuffer(IXCanvas xCanvas, IntPtr pixels, int width, int height, int rowBytes)
        {
            var info = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            using (SKSurface surface = SKSurface.Create(info, pixels, rowBytes))
            {
                // Paint all elements from the canvas on the surface
                new SkiaPainter(surface).Paint(xCanvas);
            }
        }

        public override IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            throw new NotImplementedException(
                "CreateGraphicsView isn't supported for the default SkiaXGraphicsRender; use a platform specific subclass instead");
        }
    }
}
