using System;
using XGraphics.ImageLoading;
using XGraphics.ImageLoading.Work;

namespace XGraphics
{
    public abstract class XGraphicsRenderer
    {
        /// <summary>
        /// This is the default render, which must be set prior to using XGraphics. It specifies what backend (Skia, a native backend, etc) to use by default for the rendering.
        /// </summary>
        public static XGraphicsRenderer? DefaultRenderer { get; set; }

        /// <summary>
        /// Render the graphics to the specified bitmap memory buffer.
        /// </summary>
        /// <param name="xCanvas">graphics object to render</param>
        /// <param name="imageLoader">image provider, downloading/processing ImageSources</param>
        /// <param name="pixels">buffer data</param>
        /// <param name="width">width of the bitmap, in pixels</param>
        /// <param name="height">height of the bitmap, in pixels</param>
        /// <param name="rowBytes">number of bytes per row in the buffer</param>
        public abstract void RenderToBuffer(IXCanvas xCanvas, IntPtr pixels, int width, int height, int rowBytes);

        public abstract IXGraphicsView CreateGraphicsView(object? arg1 = null, object? arg2 = null, object? arg3 = null);

        public abstract IImageLoader ImageLoader { get; set; }
    }

    public interface IXGraphicsView
    {
        IXCanvas? Content { set;  }

        object? NativeControl { get; }
    }
}
