namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IRectangle : IShape
    {
        [ModelDefaultValue(0.0)]
        double RadiusX { get; }

        [ModelDefaultValue(0.0)]
        double RadiusY { get; }
    }
}
