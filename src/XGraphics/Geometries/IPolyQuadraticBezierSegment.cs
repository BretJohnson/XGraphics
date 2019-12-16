namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyQuadraticBezierSegment : IPathSegment
    {
        Points Points { get; }
    }
}
