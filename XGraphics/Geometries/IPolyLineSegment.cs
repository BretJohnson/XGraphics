using System.Collections.Generic;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyLineSegment : IPathSegment
    {
        IEnumerable<Point> Points { get; }
    }
}
