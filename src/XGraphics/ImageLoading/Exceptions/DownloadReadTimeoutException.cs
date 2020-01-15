using System;

namespace XGraphics.ImageLoading.Exceptions
{
    public class DownloadReadTimeoutException : Exception
    {
        public DownloadReadTimeoutException() : base("Read timeout")
        {
        }
    }
}
