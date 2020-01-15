using System;

namespace XGraphics.ImageLoading.Extensions
{
    public static class DisposableExtensions
    {
        public static bool TryDispose(this IDisposable obj)
        {
            try
            {
                if (obj != null)
                {
                    obj?.Dispose();
                    return true;
                }
            }
            catch (ObjectDisposedException)
            {
            }

            return false;
        }
    }
}
