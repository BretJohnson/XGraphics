using System;

namespace XGraphics.ImageLoading.Helpers
{
    public interface IMiniLogger
    {
        void Debug(string message);

        void Error(string errorMessage);

        void Error(string errorMessage, Exception ex);
    }
}
