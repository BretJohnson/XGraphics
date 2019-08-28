using XGraphics.Transforms;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Transforms
{
    public class TranslateTransform : Transform, ITranslateTransform
    {
        public static readonly BindableProperty XProperty = PropertyUtils.Create(nameof(X), typeof(double), typeof(TranslateTransform), 0.0);
        public static readonly BindableProperty YProperty = PropertyUtils.Create(nameof(Y), typeof(double), typeof(TranslateTransform), 0.0);

        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }
    }
}