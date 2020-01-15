using System;

namespace XGraphics
{
    [GraphicsModelObject]
    public interface ILoadableImageSource : IImageSource
    {
        [ModelDefaultValue(null)]
        DataSource Source { get; }

        [ModelDefaultValue(LoadingStatus.NotStarted)]
        LoadingStatus LoadingStatus { get; }

        [ModelDefaultValue(null)]
        LoadedImage? LoadedImage { get; }

        [ModelDefaultValue(null)]
        Exception? LoadingError { get; }

#if LATER
        event DownloadProgressEventHandler DownloadProgress;
        event EventHandler LoadComplete;

        void RaiseDownloadProgress(DownloadProgressEventArgs args);

        void LoadStarted();
        void LoadSucceeded(LoadedImage loadedImage);
        void LoadFailed(Exception exception);
        void LoadCanceled();
#endif

        [ModelDefaultValue(null)]
        ImageDecoder? Decoder { get; }
    }

    public delegate void DownloadProgressEventHandler(object sender, DownloadProgressEventArgs e);

    public class DownloadProgressEventArgs : EventArgs
    {
        public DownloadProgressEventArgs(int progress)
        {
            Progress = progress;
        }

        /// <summary>
        /// Gets download progress as a value that is between 0 and 100. 0 indicates no progress; 100 indicates that the download is complete.
        /// </summary>
        public int Progress { get; }
    }
}
