namespace XGraphics.Transforms
{
    [GraphicsModelObject]
    public interface ITranslateTransform : ITransform
    {
        [ModelDefaultValue(0.0)]
        double X { get; }

        [ModelDefaultValue(0.0)]
        double Y { get; }
    }
}
