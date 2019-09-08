namespace XGraphics.Transforms
{
    [GraphicsModelObject]
    public interface IRotateTransform : ITransform
    {
        [ModelDefaultValue(0.0)]
        double Angle { get; }

        [ModelDefaultValue(0.0)]
        double CenterX { get; }

        [ModelDefaultValue(0.0)]
        double CenterY { get; }
    }
}
