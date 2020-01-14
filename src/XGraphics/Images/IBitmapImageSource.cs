namespace XGraphics
{
    [GraphicsModelObject]
    public interface IBitmapImageSource : ILoadableImageSource
    {
        [ModelDefaultValue(0)]
        int DecodePixelWidth { get; }

        [ModelDefaultValue(0)]
        int DecodePixelHeight { get; }
    }
}
