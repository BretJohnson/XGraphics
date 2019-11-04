// This file is generated from IPolyline.cs. Update the source file to change its contents.
using XGraphics.Shapes;
using System.Windows;
using System.Windows.Markup;
using System;

namespace XGraphics.WPF.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public static readonly DependencyProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(Polyline), FillRule.EvenOdd);
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Point[]), typeof(Polyline), Array.Empty<Point>());

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }

        public Point[] Points
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}