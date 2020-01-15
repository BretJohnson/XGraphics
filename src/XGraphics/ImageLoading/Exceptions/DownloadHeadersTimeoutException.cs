using System;

namespace XGraphics.ImageLoading.Exceptions
{
    public class DownloadHeadersTimeoutException : Exception
    {
        public DownloadHeadersTimeoutException() : base("Headers timeout")
        {
        }
    }
}
