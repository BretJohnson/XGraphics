namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface ISolidColorBrush : IBrush
    {
        Color Color { get; }
    }
}
