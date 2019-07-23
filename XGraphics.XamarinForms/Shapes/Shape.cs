using System;
using Xamarin.Forms;
using XGraphics.Shapes;

namespace XGraphics.XamarinForms.Shapes
{
    public class Shape : Element, IShape
    {
        public static readonly BindableProperty LeftProperty = CreateProperty(IShapeProperties.LeftProperty);
        public static readonly BindableProperty TopProperty = CreateProperty(IShapeProperties.TopProperty);
        public static readonly BindableProperty WidthProperty = CreateProperty(IShapeProperties.WidthProperty);
        public static readonly BindableProperty HeightProperty = CreateProperty(IShapeProperties.HeightProperty);
        public static readonly BindableProperty ZIndexProperty = CreateProperty(IShapeProperties.ZIndexProperty);

        public static readonly BindableProperty StrokeProperty = CreateProperty(IShapeProperties.StrokeProperty, typeof(Brush));
        public static readonly BindableProperty StrokeThicknessProperty = CreateProperty(IShapeProperties.StrokeThicknessProperty);
        public static readonly BindableProperty FillProperty = CreateProperty(IShapeProperties.FillProperty, typeof(Brush));

        private static BindableProperty CreateProperty(XPlatBindableProperty xplatProperty, Type? propertyType = null) =>
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
