// This file is generated from ILineSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly BindableProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(Wrapper.Point), typeof(LineSegment), PropertyUtils.DefaultPoint);

        Point ILineSegment.Point => Point.WrappedPoint;
        public Wrapper.Point Point
        {
            get => (Wrapper.Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
    }
}