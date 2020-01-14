using System;

namespace XGraphics.ImageLoading.Helpers
{
    public class ConsoleMiniLogger : IMiniLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        public void Error(string errorMessage, Exception ex)
        {
            Error(errorMessage + Environment.NewLine + ex.ToString());
        }
    }
}
