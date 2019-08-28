using System.Collections.Generic;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyBezierSegment : IPathSegment
    {
        IEnumerable<Point> Points { get; }
    }
}
