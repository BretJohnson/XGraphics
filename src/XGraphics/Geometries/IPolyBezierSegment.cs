namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyBezierSegment : IPathSegment
    {
        Points Points { get; }
    }
}
