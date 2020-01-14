namespace XGraphics.ImageLoading.Work
{
    public enum LoadingResult
    {
        // Errors
        NotFound = -1,
        InvalidTarget = -2,
        Canceled = -3,
        Failed = -4,

        // Success results
        MemoryCache = 10,
        DiskCache = 11,
        Internet = 12,

        Disk = 20,
        ApplicationBundle = 21,
        CompiledResource = 22,
        EmbeddedResource = 23,

        Stream = 30,
    }
}
