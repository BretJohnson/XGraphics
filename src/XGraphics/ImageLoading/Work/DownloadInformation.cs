using System;

namespace XGraphics.ImageLoading.Work
{
    public class DownloadInformation
    {
        public DownloadInformation(TimeSpan cacheValidity)
        {
            CacheValidity = cacheValidity;
        }

        public TimeSpan CacheValidity { get; internal set; }
    }
}
