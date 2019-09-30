// This file is generated from IBezierSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class BezierSegment : PathSegment, IBezierSegment
    {
        public static readonly BindableProperty Point1Property = PropertyUtils.Create(nameof(Point1), typeof(Wrapper.Point), typeof(BezierSegment), Wrapper.Point.Default);
        public static readonly BindableProperty Point2Property = PropertyUtils.Create(nameof(Point2), typeof(Wrapper.Point), typeof(BezierSegment), Wrapper.Point.Default);
        public static readonly BindableProperty Point3Property = PropertyUtils.Create(nameof(Point3), typeof(Wrapper.Point), typeof(BezierSegment), Wrapper.Point.Default);

        Point IBezierSegment.Point1 => Point1.WrappedPoint;
        public Wrapper.Point Point1
        {
            get => (Wrapper.Point)GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }

        Point IBezierSegment.Point2 => Point2.WrappedPoint;
        public Wrapper.Point Point2
        {
            get => (Wrapper.Point)GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }

        Point IBezierSegment.Point3 => Point3.WrappedPoint;
        public Wrapper.Point Point3
        {
            get => (Wrapper.Point)GetValue(Point3Property);
            set => SetValue(Point3Property, value);
        }
    }
}