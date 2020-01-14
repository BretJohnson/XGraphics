using System.IO;
using XGraphics.ImageLoading.Work;

namespace XGraphics.ImageLoading
{
    public class ResolvedDataSource
    {
        public ResolvedDataSource(Stream stream, LoadingResult loadingResult, ImageInformation imageInformation)
        {
            Stream = stream;
            LoadingResult = loadingResult;
            ImageInformation = imageInformation;
        }

        public Stream Stream { get; }

        public LoadingResult LoadingResult { get; }

        public ImageInformation ImageInformation { get; }
    }
}
