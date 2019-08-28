namespace XGraphics
{
    [GraphicsModelObject]
    public interface ISolidColorBrush : IBrush
    {
        Color Color { get; }
    }
}
