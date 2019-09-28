// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public static readonly BindableProperty Point1Property = PropertyUtils.Create(nameof(Point1), typeof(Wrapper.Point), typeof(QuadraticBezierSegment), PropertyUtils.DefaultPoint);
        public static readonly BindableProperty Point2Property = PropertyUtils.Create(nameof(Point2), typeof(Wrapper.Point), typeof(QuadraticBezierSegment), PropertyUtils.DefaultPoint);

        Point IQuadraticBezierSegment.Point1 => Point1.WrappedPoint;
        public Wrapper.Point Point1
        {
            get => (Wrapper.Point)GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }

        Point IQuadraticBezierSegment.Point2 => Point2.WrappedPoint;
        public Wrapper.Point Point2
        {
            get => (Wrapper.Point)GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
    }
}