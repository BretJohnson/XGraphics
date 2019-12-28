namespace XGraphics
{
    [GraphicsModelObject]
    public interface IImage : IGraphicsElement
    {
        [ModelDefaultValue(null)]
        ImageSource Source { get; }
    }
}
