namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface ILinearGradientBrush : IGradientBrush
    {
        Point StartPoint { get; }

        Point EndPoint { get; }
    }
}
