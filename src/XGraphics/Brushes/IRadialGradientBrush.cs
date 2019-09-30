namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IRadialGradientBrush : IGradientBrush
    {
        [ModelDefaultValue("0.5,0.5")]
        Point Center { get; }

        [ModelDefaultValue("0.5,0.5")]
        Point GradientOrigin { get; }

        [ModelDefaultValue(0.5)]
        double RadiusX { get; }
    }
}
