using System;
using XGraphics.ImageLoading;

namespace XGraphics
{
    [GraphicsModelObject]
    public interface ILoadableImageSource : IImageSource
    {
        [ModelDefaultValue(null)]
        DataSource Source { get; }

        [ModelDefaultValue(null)]
        LoadedImage? LoadedImage { get; }

        DownloadProgressEventHandler? DownloadProgress { get; set; }
        event EventHandler? LoadComplete;

        void LoadSucceeded(LoadedImage loadedImage);
        void LoadFailed(Exception exception);

        void RaiseDownloadProgress(DownloadProgressEventArgs args);

        [ModelDefaultValue(null)]
        ImageDecoder Decoder { get; }
    }

    public delegate void DownloadProgressEventHandler(object sender, DownloadProgressEventArgs e);

    public class DownloadProgressEventArgs : EventArgs
    {
        public DownloadProgressEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        public int Current { get; }

        public int Total { get; }
    }
}
