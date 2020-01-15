using System;

namespace XGraphics
{
    public class BitmapLoadedImage : LoadedImage
    {
        public BitmapLoadedImage(IDisposable imageObject, int pixelWidth, int pixelHeight)
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
