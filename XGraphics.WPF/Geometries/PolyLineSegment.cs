using System.Collections.Generic;
using XGraphics.Geometries;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PolyLineSegment : PathSegment, IPolyLineSegment
    {
        public PolyLineSegment()
        {
            Points = new PointCollection();
            Points.Changed += OnSubobjectChanged;
        }

        IEnumerable<Point> IPolyLineSegment.Points => Points;
        public PointCollection Points
        {
            get;
        }
    }
}