// This file is generated from IGraphicsElement.cs. Update the source file to change its contents.

using XGraphics.Shapes;
using XGraphics.WPF.Shapes;
using XGraphics.Transforms;
using XGraphics.WPF.Transforms;
using XGraphics;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF
{
    public class GraphicsElement : DependencyObjectWithCascadingNotifications, IGraphicsElement
    {
        public static readonly DependencyProperty LeftProperty = PropertyUtils.Create(nameof(Left), typeof(double), typeof(GraphicsElement), 0.0);
        public static readonly DependencyProperty TopProperty = PropertyUtils.Create(nameof(Top), typeof(double), typeof(GraphicsElement), 0.0);
        public static readonly DependencyProperty WidthProperty = PropertyUtils.Create(nameof(Width), typeof(double), typeof(GraphicsElement), double.NaN);
        public static readonly DependencyProperty HeightProperty = PropertyUtils.Create(nameof(Height), typeof(double), typeof(GraphicsElement), double.NaN);
        public static readonly DependencyProperty RenderTransformProperty = PropertyUtils.Create(nameof(RenderTransform), typeof(Transform), typeof(GraphicsElement), null);

        public double Left
        {
            get => (double)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public double Top
        {
            get => (double)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }

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

        public Transform? RenderTransform
        {
            get => (Transform?)GetValue(RenderTransformProperty);
            set => SetValue(RenderTransformProperty, value);
        }
        ITransform? IGraphicsElement.RenderTransform => RenderTransform;
    }
}