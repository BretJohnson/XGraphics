using System;

namespace XGraphics
{
    public class DownloadException : Exception
    {
        public DownloadException(string message) : base(message)
        {
        }
    }
}
