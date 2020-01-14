namespace XGraphics
{
    [GraphicsModelObject]
    public interface IImage : IGraphicsElement
    {
        [ModelDefaultValue(null)]
        IImageSource Source { get; }
    }
}
