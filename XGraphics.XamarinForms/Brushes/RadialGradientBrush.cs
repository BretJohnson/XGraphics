using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly BindableProperty CenterProperty = PropertyUtils.Create(nameof(Center), typeof(Wrapper.Point), typeof(RadialGradientBrush), PropertyUtils.DefaultPoint);
        public static readonly BindableProperty GraidentOriginProperty = PropertyUtils.Create(nameof(GraidentOrigin), typeof(Wrapper.Point), typeof(RadialGradientBrush), PropertyUtils.DefaultPoint);
        public static readonly BindableProperty RadiusXProperty = PropertyUtils.Create(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5);
        public static readonly BindableProperty RadiusYProperty = PropertyUtils.Create(nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), 0.5);

        // The default value is 0.5, 0.5
        Point IRadialGradientBrush.Center => Center.WrappedPoint;
        public Wrapper.Point Center
        {
            get => (Wrapper.Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        // The default value is 0.5, 0.5
        Point IRadialGradientBrush.GraidentOrigin => GraidentOrigin.WrappedPoint;
        public Wrapper.Point GraidentOrigin
        {
            get => (Wrapper.Point)GetValue(GraidentOriginProperty);
            set => SetValue(GraidentOriginProperty, value);
        }

        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }
    }
}