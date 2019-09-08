using XGraphics.Brushes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Brushes
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly DependencyProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(Wrapper.Point), typeof(LinearGradientBrush), PropertyUtils.DefaultPoint);
        public static readonly DependencyProperty EndPointProperty = PropertyUtils.Create(nameof(EndPoint), typeof(Wrapper.Point), typeof(LinearGradientBrush), PropertyUtils.DefaultPoint);

        Point ILinearGradientBrush.StartPoint => StartPoint.WrappedPoint;
        public Wrapper.Point StartPoint
        {
            get => (Wrapper.Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        Point ILinearGradientBrush.EndPoint => EndPoint.WrappedPoint;
        public Wrapper.Point EndPoint
        {
            get => (Wrapper.Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
    }
}