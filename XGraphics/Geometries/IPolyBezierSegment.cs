namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyBezierSegment : IPathSegment
    {
        Point[] Points { get; }
    }
}
