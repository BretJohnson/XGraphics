// This file is generated from IShape.cs. Update the source file to change its contents.
using XGraphics.Brushes;
using XGraphics.XamarinForms.Brushes;
using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Shape : GraphicsElement, IShape
    {
        public static readonly BindableProperty WidthProperty = PropertyUtils.Create(nameof(Width), typeof(double), typeof(Shape), double.NaN);
        public static readonly BindableProperty HeightProperty = PropertyUtils.Create(nameof(Height), typeof(double), typeof(Shape), double.NaN);
        public static readonly BindableProperty StrokeProperty = PropertyUtils.Create(nameof(Stroke), typeof(Brush), typeof(Shape), null);
        public static readonly BindableProperty StrokeThicknessProperty = PropertyUtils.Create(nameof(StrokeThickness), typeof(double), typeof(Shape), 1.0);
        public static readonly BindableProperty StrokeMiterLimitProperty = PropertyUtils.Create(nameof(StrokeMiterLimit), typeof(double), typeof(Shape), 10.0);
        public static readonly BindableProperty FillProperty = PropertyUtils.Create(nameof(Fill), typeof(Brush), typeof(Shape), null);

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
            get => (Brush?)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public double StrokeMiterLimit
        {
            get => (double)GetValue(StrokeMiterLimitProperty);
            set => SetValue(StrokeMiterLimitProperty, value);
        }

        IBrush? IShape.Fill => Fill;
        public Brush? Fill
        {
            get => (Brush?)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
    }
}