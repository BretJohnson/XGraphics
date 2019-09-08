using XGraphics.Geometries;
using Xamarin.Forms;
using System;

namespace XGraphics.XamarinForms.Geometries
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Point[]), typeof(PolyBezierSegment), Array.Empty<Point>());

        public Point[] Points
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}