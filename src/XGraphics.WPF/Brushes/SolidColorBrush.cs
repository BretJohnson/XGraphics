using XGraphics.Brushes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Brushes
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly DependencyProperty ColorProperty = PropertyUtils.Create(nameof(Color), typeof(Wrapper.Color), typeof(SolidColorBrush), PropertyUtils.DefaultColor);

        Color ISolidColorBrush.Color => Color.WrappedColor;
        public Wrapper.Color Color
        {
            get => (Wrapper.Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}