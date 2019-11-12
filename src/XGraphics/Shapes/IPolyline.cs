namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IPolyline : IShape
    {
        [ModelDefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; }

        Points Points { get; }
    }
}
