// This file is generated from IPolyLineSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;
using System;

namespace XGraphics.WPF.Geometries
{
    public class PolyLineSegment : PathSegment, IPolyLineSegment
    {
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Point[]), typeof(PolyLineSegment), Array.Empty<Point>());

        public Point[] Points
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}