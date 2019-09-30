// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.
using XGraphics.Brushes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Brushes
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly DependencyProperty CenterProperty = PropertyUtils.Create(nameof(Center), typeof(Wrapper.Point), typeof(RadialGradientBrush), Wrapper.Point.CenterDefault);
        public static readonly DependencyProperty GradientOriginProperty = PropertyUtils.Create(nameof(GradientOrigin), typeof(Wrapper.Point), typeof(RadialGradientBrush), Wrapper.Point.CenterDefault);
        public static readonly DependencyProperty RadiusXProperty = PropertyUtils.Create(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5);

        Point IRadialGradientBrush.Center => Center.WrappedPoint;
        public Wrapper.Point Center
        {
            get => (Wrapper.Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        Point IRadialGradientBrush.GradientOrigin => GradientOrigin.WrappedPoint;
        public Wrapper.Point GradientOrigin
        {
            get => (Wrapper.Point)GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }

        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
    }
}