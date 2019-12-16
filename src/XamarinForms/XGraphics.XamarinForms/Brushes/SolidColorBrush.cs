// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly BindableProperty ColorProperty = PropertyUtils.Create(nameof(Color), typeof(Wrapper.Color), typeof(SolidColorBrush), Wrapper.Color.Default);

        public Wrapper.Color Color
        {
            get => (Wrapper.Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color ISolidColorBrush.Color => Color.WrappedColor;
    }
}