using System;
using System.Windows;
using XGraphics.Shapes;

namespace XGraphics.WPF.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly DependencyProperty RadiusXProperty = CreateProperty(IRectangleProperties.RadiusXProperty);
        public static readonly DependencyProperty RadiusYProperty = CreateProperty(IRectangleProperties.RadiusYProperty);

        private static DependencyProperty CreateProperty(XPlatBindableProperty xplatProperty, Type? propertyType = null) =>
            PropertyUtils.CreateProperty(xplatProperty, typeof(Rectangle), propertyType);


        public double RadiusX {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        public double RadiusY {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }
    }
}
