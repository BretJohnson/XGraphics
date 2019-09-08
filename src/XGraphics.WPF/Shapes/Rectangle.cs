using XGraphics.Shapes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly DependencyProperty RadiusXProperty = PropertyUtils.Create(nameof(RadiusX), typeof(double), typeof(Rectangle), 0.0);
        public static readonly DependencyProperty RadiusYProperty = PropertyUtils.Create(nameof(RadiusY), typeof(double), typeof(Rectangle), 0.0);

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