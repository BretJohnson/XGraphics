using System;
using System.Windows;
using XGraphics.Shapes;

namespace XGraphics.WPF.Shapes
{
    public class Shape : DependencyObject, IShape
    {
        public static readonly DependencyProperty LeftProperty = CreateProperty(IShapeProperties.LeftProperty);
        public static readonly DependencyProperty TopProperty = CreateProperty(IShapeProperties.TopProperty);
        public static readonly DependencyProperty WidthProperty = CreateProperty(IShapeProperties.WidthProperty);
        public static readonly DependencyProperty HeightProperty = CreateProperty(IShapeProperties.HeightProperty);
        public static readonly DependencyProperty ZIndexProperty = CreateProperty(IShapeProperties.ZIndexProperty);

        public static readonly DependencyProperty StrokeProperty = CreateProperty(IShapeProperties.StrokeProperty, typeof(Brush));
        public static readonly DependencyProperty StrokeThicknessProperty = CreateProperty(IShapeProperties.StrokeThicknessProperty);
        public static readonly DependencyProperty FillProperty = CreateProperty(IShapeProperties.FillProperty, typeof(Brush));

        private static DependencyProperty CreateProperty(XPlatBindableProperty xplatProperty, Type? propertyType = null) =>
            PropertyUtils.CreateProperty(xplatProperty, typeof(Shape), propertyType);

        public double Left {
            get => (double)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public double Top { 
            get => (double)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }

        public double Width {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public double Height {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public int ZIndex {
            get => (int)GetValue(ZIndexProperty);
            set => SetValue(ZIndexProperty, value);
        }
        
        IBrush? IShape.Stroke => Stroke;

        public Brush? Stroke {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        IBrush? IShape.Fill => Fill;

        public Brush? Fill {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
    }
}
