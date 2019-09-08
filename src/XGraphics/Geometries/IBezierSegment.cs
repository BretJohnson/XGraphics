namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IBezierSegment : IPathSegment
    {
        Point Point1 { get; }

        Point Point2 { get; }

        Point Point3 { get; }
    }
}
