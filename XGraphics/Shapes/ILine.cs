namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface ILine : IShape
    {
        [ModelDefaultValue(0.0)]
        double X1 { get; }

        [ModelDefaultValue(0.0)]
        double Y1 { get; }

        [ModelDefaultValue(0.0)]
        double X2 { get; }

        [ModelDefaultValue(0.0)]
        double Y2 { get; }
    }
}
