namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyLineSegment : IPathSegment
    {
        Points Points { get; }
    }
}
