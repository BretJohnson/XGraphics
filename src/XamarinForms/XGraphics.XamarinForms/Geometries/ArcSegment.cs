// This file is generated from IArcSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public static readonly BindableProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(Wrapper.Point), typeof(ArcSegment), Wrapper.Point.Default);
        public static readonly BindableProperty SizeProperty = PropertyUtils.Create(nameof(Size), typeof(Wrapper.Size), typeof(ArcSegment), Wrapper.Size.Default);
        public static readonly BindableProperty RotationAngleProperty = PropertyUtils.Create(nameof(RotationAngle), typeof(double), typeof(ArcSegment), 0.0);
        public static readonly BindableProperty IsLargeArcProperty = PropertyUtils.Create(nameof(IsLargeArc), typeof(bool), typeof(ArcSegment), false);
        public static readonly BindableProperty SweepDirectionProperty = PropertyUtils.Create(nameof(SweepDirection), typeof(SweepDirection), typeof(ArcSegment), SweepDirection.Counterclockwise);

        public Wrapper.Point Point
        {
            get => (Wrapper.Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point IArcSegment.Point => Point.WrappedPoint;

        public Wrapper.Size Size
        {
            get => (Wrapper.Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        Size IArcSegment.Size => Size.WrappedSize;

        public double RotationAngle
        {
            get => (double)GetValue(RotationAngleProperty);
            set => SetValue(RotationAngleProperty, value);
        }

        public bool IsLargeArc
        {
            get => (bool)GetValue(IsLargeArcProperty);
            set => SetValue(IsLargeArcProperty, value);
        }

        public SweepDirection SweepDirection
        {
            get => (SweepDirection)GetValue(SweepDirectionProperty);
            set => SetValue(SweepDirectionProperty, value);
        }
    }
}