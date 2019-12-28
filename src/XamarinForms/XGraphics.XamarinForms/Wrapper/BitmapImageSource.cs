namespace XGraphics.XamarinForms.Wrapper
{
    public class BitmapImageSource : ImageSource
    {
        public XGraphics.BitmapImageSource WrappedBitmapImageSource { get; }

        public override XGraphics.ImageSource WrappedImageSource => WrappedBitmapImageSource;

        public BitmapImageSource(XGraphics.BitmapImageSource wrappedBitmapImageSource)
        {
            WrappedBitmapImageSource = wrappedBitmapImageSource;
        }
    }
}
