// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.

using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly BindableProperty CenterProperty = PropertyUtils.Create(nameof(Center), typeof(Wrapper.Point), typeof(RadialGradientBrush), Wrapper.Point.CenterDefault);
        public static readonly BindableProperty GradientOriginProperty = PropertyUtils.Create(nameof(GradientOrigin), typeof(Wrapper.Point), typeof(RadialGradientBrush), Wrapper.Point.CenterDefault);
        public static readonly BindableProperty RadiusXProperty = PropertyUtils.Create(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5);

        public Wrapper.Point Center
        {
            get => (Wrapper.Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        Point IRadialGradientBrush.Center => Center.WrappedPoint;

        public Wrapper.Point GradientOrigin
        {
            get => (Wrapper.Point)GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }
        Point IRadialGradientBrush.GradientOrigin => GradientOrigin.WrappedPoint;

        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
    }
}