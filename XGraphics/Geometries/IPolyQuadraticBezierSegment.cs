using System.Collections.Generic;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyQuadraticBezierSegment : IPathSegment
    {
        IEnumerable<Point> Points { get; }
    }
}
