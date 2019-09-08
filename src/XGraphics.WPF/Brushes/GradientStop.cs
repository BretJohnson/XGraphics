using XGraphics.Brushes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Brushes
{
    public class GradientStop : DependencyObjectWithCascadingNotifications, IGradientStop
    {
        public static readonly DependencyProperty ColorProperty = PropertyUtils.Create(nameof(Color), typeof(Wrapper.Color), typeof(GradientStop), PropertyUtils.DefaultColor);
        public static readonly DependencyProperty OffsetProperty = PropertyUtils.Create(nameof(Offset), typeof(double), typeof(GradientStop), 0.0);

        // The default is Transparent
        Color IGradientStop.Color => Color.WrappedColor;
        public Wrapper.Color Color
        {
            get => (Wrapper.Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
    }
}