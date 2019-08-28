using System.Collections.Generic;
using XGraphics.Geometries;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PolyQuadraticBezierSegment : PathSegment, IPolyQuadraticBezierSegment
    {
        public PolyQuadraticBezierSegment()
        {
            Points = new PointCollection();
            Points.Changed += OnSubobjectChanged;
        }

        IEnumerable<Point> IPolyQuadraticBezierSegment.Points => Points;
        public PointCollection Points
        {
            get;
        }
    }
}