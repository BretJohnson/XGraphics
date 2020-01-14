namespace XGraphics.ImageLoading.Helpers
{
    public interface IPlatformPerformance
    {
        int GetCurrentManagedThreadId();

        int GetCurrentSystemThreadId();

        string GetMemoryInfo();
    }
}

