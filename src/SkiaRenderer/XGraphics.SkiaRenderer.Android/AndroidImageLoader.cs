using System.IO;
using XGraphics.ImageLoading;
using XGraphics.ImageLoading.Cache;

namespace XGraphics.SkiaRenderer.Android
{
    public class AndroidImageLoader : ImageLoader
    {
        public AndroidImageLoader()
        {
            var context = new global::Android.Content.ContextWrapper(global::Android.App.Application.Context);
            string tmpPath = context.CacheDir.AbsolutePath;
            string cachePath = Path.Combine(tmpPath, "XGraphicsDiskCache");

            // Default the disk cache path. The client can override this if they want
            DiskCachePath = cachePath;
        }

        protected override IDiskCache CreatePlatformDiskCache()
        {
            Java.IO.File diskCacheDirectory = new Java.IO.File(DiskCachePath);
            if (!diskCacheDirectory.Exists())
                diskCacheDirectory.Mkdirs();

            if (!diskCacheDirectory.CanRead())
                diskCacheDirectory.SetReadable(true, false);

            if (!diskCacheDirectory.CanWrite())
                diskCacheDirectory.SetWritable(true, false);

            return new SimpleDiskCache(this, DiskCachePath);
        }
    }
}
