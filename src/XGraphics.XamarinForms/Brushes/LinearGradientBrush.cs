// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.
using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(Wrapper.Point), typeof(LinearGradientBrush), PropertyUtils.DefaultPoint);
        public static readonly BindableProperty EndPointProperty = PropertyUtils.Create(nameof(EndPoint), typeof(Wrapper.Point), typeof(LinearGradientBrush), PropertyUtils.DefaultPoint);

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