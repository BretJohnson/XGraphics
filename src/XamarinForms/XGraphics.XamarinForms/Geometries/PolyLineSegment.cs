// This file is generated from IPolyLineSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using Xamarin.Forms;
using System;

namespace XGraphics.XamarinForms.Geometries
{
    public class PolyLineSegment : PathSegment, IPolyLineSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Point[]), typeof(PolyLineSegment), Array.Empty<Point>());

        public Point[] Points
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}