// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly DependencyProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(Wrapper.Point), typeof(LineSegment), Wrapper.Point.Default);

        public Wrapper.Point Point
        {
            get => (Wrapper.Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point ILineSegment.Point => Point.WrappedPoint;
    }
}