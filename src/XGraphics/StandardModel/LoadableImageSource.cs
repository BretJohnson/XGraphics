// This file is generated from ILoadableImageSource.cs. Update the source file to change its contents.

using System;
using XGraphics;

namespace XGraphics.StandardModel
{
    public class LoadableImageSource : ImageSource, ILoadableImageSource
    {
        public DataSource Source { get; set; } = null;

        public LoadingStatus LoadingStatus { get; set; } = LoadingStatus.NotStarted;

        public LoadedImage? LoadedImage { get; set; } = null;

        public Exception? LoadingError { get; set; } = null;

        public ImageDecoder? Decoder { get; set; } = null;
    }
}