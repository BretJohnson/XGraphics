using System;
using Xamarin.Forms;
using XGraphics.Shapes;

namespace XGraphics.XamarinForms.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly BindableProperty RadiusXProperty = CreateProperty(IRectangleProperties.RadiusXProperty);
        public static readonly BindableProperty RadiusYProperty = CreateProperty(IRectangleProperties.RadiusYProperty);

        private static BindableProperty CreateProperty(XPlatBindableProperty xplatProperty, Type? propertyType = null) =>
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
