using XGraphics.Transforms;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Transforms
{
    public class ScaleTransform : Transform, IScaleTransform
    {
        public static readonly BindableProperty CenterXProperty = PropertyUtils.Create(nameof(CenterX), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly BindableProperty CenterYProperty = PropertyUtils.Create(nameof(CenterY), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly BindableProperty ScaleXProperty = PropertyUtils.Create(nameof(ScaleX), typeof(double), typeof(ScaleTransform), 1.0);
        public static readonly BindableProperty ScaleYProperty = PropertyUtils.Create(nameof(ScaleY), typeof(double), typeof(ScaleTransform), 1.0);

        public double CenterX
        {
            get => (double)GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }

        public double CenterY
        {
            get => (double)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        public double ScaleX
        {
            get => (double)GetValue(ScaleXProperty);
            set => SetValue(ScaleXProperty, value);
        }

        public double ScaleY
        {
            get => (double)GetValue(ScaleYProperty);
            set => SetValue(ScaleYProperty, value);
        }
    }
}