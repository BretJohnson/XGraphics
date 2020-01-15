using System;

namespace XGraphics.ImageLoading.Exceptions
{
    public class DownloadException : Exception
    {
        public DownloadException(string message) : base(message)
        {
        }
    }
}
