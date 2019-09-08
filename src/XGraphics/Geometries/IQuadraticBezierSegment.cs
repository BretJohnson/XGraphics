namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IQuadraticBezierSegment : IPathSegment
    {
        Point Point1 { get; }

        Point Point2 { get; }
    }
}
