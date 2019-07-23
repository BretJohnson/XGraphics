using System;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly BindableProperty ColorProperty = CreateProperty(SolidColorBrushProperties.ColorProperty);

        private static BindableProperty CreateProperty(XPlatBindableProperty xplatProperty, Type? propertyType = null) =>
            PropertyUtils.CreateProperty(xplatProperty, typeof(SolidColorBrush), propertyType);

        public Color Color {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
