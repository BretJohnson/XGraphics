using System.Collections.Generic;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
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