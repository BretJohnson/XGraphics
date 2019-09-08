namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyQuadraticBezierSegment : IPathSegment
    {
        Point[] Points { get; }
    }
}
