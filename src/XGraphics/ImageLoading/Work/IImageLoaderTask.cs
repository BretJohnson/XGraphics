using System;
using System.Threading.Tasks;
using XGraphics.StandardModel;

namespace XGraphics.ImageLoading.Work
{
    public interface IImageLoaderTask : IScheduledWork, IDisposable
    {
        ILoadableImageSource ImageSource { get; }

        int? Priority { get; set; }

        bool CanUseMemoryCache { get; }

        string Key { get; }

        string KeyRaw { get; }

        bool TryLoadFromMemoryCache();

        Task RunAsync();
    }
}

