// This file is generated from IPolyBezierSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;
using System;

namespace XGraphics.WPF.Geometries
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Point[]), typeof(PolyBezierSegment), Array.Empty<Point>());

        public Point[] Points
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}