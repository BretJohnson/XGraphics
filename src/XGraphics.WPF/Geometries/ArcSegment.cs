// This file is generated from IArcSegment.cs. Update the source file to change its contents.
using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public static readonly DependencyProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(Wrapper.Point), typeof(ArcSegment), PropertyUtils.DefaultPoint);
        public static readonly DependencyProperty SizeProperty = PropertyUtils.Create(nameof(Size), typeof(Wrapper.Size), typeof(ArcSegment), PropertyUtils.DefaultSize);
        public static readonly DependencyProperty RotationAngleProperty = PropertyUtils.Create(nameof(RotationAngle), typeof(double), typeof(ArcSegment), 0.0);
        public static readonly DependencyProperty IsLargeArcProperty = PropertyUtils.Create(nameof(IsLargeArc), typeof(bool), typeof(ArcSegment), false);
        public static readonly DependencyProperty SweepDirectionProperty = PropertyUtils.Create(nameof(SweepDirection), typeof(SweepDirection), typeof(ArcSegment), SweepDirection.Counterclockwise);

        Point IArcSegment.Point => Point.WrappedPoint;
        public Wrapper.Point Point
        {
            get => (Wrapper.Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }

        Size IArcSegment.Size => Size.WrappedSize;
        public Wrapper.Size Size
        {
            get => (Wrapper.Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

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