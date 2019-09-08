using XGraphicsSize = XGraphics.Size;

namespace XGraphics.WPF.Wrapper
{
    public struct Size
    {
        public static readonly Size Default = new Size(XGraphicsSize.Default);


        public XGraphicsSize WrappedSize { get; }

        public Size(XGraphicsSize wrappedSize)
        {
            WrappedSize = wrappedSize;
        }
    }
}
