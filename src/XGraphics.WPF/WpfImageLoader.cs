using XGraphics.ImageLoading;

namespace XGraphics.WPF
{
    public class WpfImageLoader : ImageLoader
    {
        public WpfImageLoader()
        {
            ClearMemoryCacheOnOutOfMemory = false;
        }
    }
}
