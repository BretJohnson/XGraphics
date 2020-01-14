using System;

namespace XGraphics.ImageLoading
{
    public class RasterLoadedImage : LoadedImage
    {
        public RasterLoadedImage(IDisposable imageObject, int pixelWidth, int pixelHeight)
        {
            ImageObject = imageObject;
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
        }

        public IDisposable ImageObject { get; }

        public int PixelWidth { get; }

        public int PixelHeight { get; }
    }
}
