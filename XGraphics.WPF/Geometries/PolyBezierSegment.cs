using System.Collections.Generic;
using XGraphics.Geometries;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public PolyBezierSegment()
        {
            Points = new PointCollection();
            Points.Changed += OnSubobjectChanged;
        }

        IEnumerable<Point> IPolyBezierSegment.Points => Points;
        public PointCollection Points
        {
            get;
        }
    }
}