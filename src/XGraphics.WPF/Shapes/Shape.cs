using XGraphics.Brushes;
using XGraphics.WPF.Brushes;
using XGraphics.Shapes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Shapes
{
    public class Shape : GraphicsElement, IShape
    {
        public static readonly DependencyProperty WidthProperty = PropertyUtils.Create(nameof(Width), typeof(double), typeof(Shape), double.NaN);
        public static readonly DependencyProperty HeightProperty = PropertyUtils.Create(nameof(Height), typeof(double), typeof(Shape), double.NaN);
        public static readonly DependencyProperty StrokeProperty = PropertyUtils.Create(nameof(Stroke), typeof(Brush), typeof(Shape), null);
        public static readonly DependencyProperty StrokeThicknessProperty = PropertyUtils.Create(nameof(StrokeThickness), typeof(double), typeof(Shape), 1.0);
        public static readonly DependencyProperty FillProperty = PropertyUtils.Create(nameof(Fill), typeof(Brush), typeof(Shape), null);

        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        IBrush? IShape.Stroke => Stroke;
        public Brush? Stroke
        {
            get => (Brush? )GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        IBrush? IShape.Fill => Fill;
        public Brush? Fill
        {
            get => (Brush? )GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
    }
}