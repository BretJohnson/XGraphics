namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IPolygon : IShape
    {
        [ModelDefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; }

        Points Points { get; }
    }
}
