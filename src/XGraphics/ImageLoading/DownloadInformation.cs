using System;

namespace XGraphics.ImageLoading
{
    public struct DownloadInformation
    {
        public DownloadInformation(TimeSpan cacheValidity)
        {
            CacheValidity = cacheValidity;
        }

        public TimeSpan CacheValidity { get; internal set; }
    }
}
