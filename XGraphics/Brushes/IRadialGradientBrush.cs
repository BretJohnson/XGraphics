namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IRadialGradientBrush : IGradientBrush
    {
        // The default value is 0.5, 0.5
        Point Center { get; }

        // The default value is 0.5, 0.5
        Point GraidentOrigin { get; }

        [ModelDefaultValue(0.5)]
        double RadiusX { get; }

        [ModelDefaultValue(0.5)]
        double RadiusY { get; }
    }
}
