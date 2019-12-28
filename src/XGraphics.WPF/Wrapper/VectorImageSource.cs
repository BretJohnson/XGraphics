namespace XGraphics.WPF.Wrapper
{
    public abstract class VectorImageSource : ImageSource
    {
        public abstract XGraphics.VectorImageSource WrappedVectorImageSource { get; }
    }
}
