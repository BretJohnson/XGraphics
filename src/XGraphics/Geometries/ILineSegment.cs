namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface ILineSegment : IPathSegment
    {
        Point Point { get; }
    }
}
