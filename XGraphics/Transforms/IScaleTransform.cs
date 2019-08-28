namespace XGraphics.Transforms
{
    [GraphicsModelObject]
    public interface IScaleTransform : ITransform
    {
        [ModelDefaultValue(0.0)]
        double CenterX { get; }

        [ModelDefaultValue(0.0)]
        double CenterY { get; }

        [ModelDefaultValue(1.0)]
        double ScaleX { get; }

        [ModelDefaultValue(1.0)]
        double ScaleY { get; }
    }
}
