using System;

namespace XGraphics
{
    public class VectorImageSource : ImageSource
    {
        public VectorImageSource(Uri uriSource) => UriSource = uriSource;

        public Uri UriSource { get; }
    }
}
