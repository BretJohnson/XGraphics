using System.Collections.Generic;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
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