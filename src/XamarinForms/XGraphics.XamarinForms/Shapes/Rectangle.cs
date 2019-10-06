// This file is generated from IRectangle.cs. Update the source file to change its contents.
using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly BindableProperty RadiusXProperty = PropertyUtils.Create(nameof(RadiusX), typeof(double), typeof(Rectangle), 0.0);
        public static readonly BindableProperty RadiusYProperty = PropertyUtils.Create(nameof(RadiusY), typeof(double), typeof(Rectangle), 0.0);

        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }
    }
}