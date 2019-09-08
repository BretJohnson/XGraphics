using System.Collections.Generic;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPolyLineSegment : IPathSegment
    {
        Point[] Points { get; }
    }
}
