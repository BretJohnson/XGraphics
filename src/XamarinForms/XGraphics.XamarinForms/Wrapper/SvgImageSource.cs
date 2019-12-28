namespace XGraphics.XamarinForms.Wrapper
{
    public class SvgImageSource : VectorImageSource
    {
        public XGraphics.SvgImageSource WrappedSvgImageSource { get; }

        public override XGraphics.VectorImageSource WrappedVectorImageSource => WrappedSvgImageSource;

        public override XGraphics.ImageSource WrappedImageSource => WrappedSvgImageSource;

        public SvgImageSource(XGraphics.SvgImageSource wrappedSvgImageSource)
        {
            WrappedSvgImageSource = wrappedSvgImageSource;
        }
    }
}
