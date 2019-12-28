using System;

namespace XGraphics
{
    public class BitmapImageSource : ImageSource
    {
        public BitmapImageSource(Uri uriSource)
        {
            UriSource = uriSource;
        }

        public Uri UriSource { get; }
    }
}
