using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly DependencyProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(Wrapper.Point), typeof(LineSegment), PropertyUtils.DefaultPoint);

        Point ILineSegment.Point => Point.WrappedPoint;
        public Wrapper.Point Point
        {
            get => (Wrapper.Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
    }
}