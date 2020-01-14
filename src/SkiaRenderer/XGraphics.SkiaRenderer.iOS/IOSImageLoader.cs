using System;
using System.IO;
using XGraphics.ImageLoading;
using XGraphics.ImageLoading.Cache;

namespace XGraphics.SkiaRenderer.iOS
{
    public class IOSImageLoader : ImageLoader
    {
        public IOSImageLoader()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string tmpPath = Path.Combine(documents, "..", "Library", "Caches");
            string cachePath = Path.Combine(tmpPath, "XGraphicsDiskCache");

            // Default the disk cache path. The client can override this if they want
            DiskCachePath = cachePath;
        }
    }
}
