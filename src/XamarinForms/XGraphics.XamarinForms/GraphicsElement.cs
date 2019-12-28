// This file is generated from IGraphicsElement.cs. Update the source file to change its contents.

using XGraphics.Shapes;
using XGraphics.XamarinForms.Shapes;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;
using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class GraphicsElement : BindableObjectWithCascadingNotifications, IGraphicsElement
    {
        public static readonly BindableProperty LeftProperty = PropertyUtils.Create(nameof(Left), typeof(double), typeof(GraphicsElement), 0.0);
        public static readonly BindableProperty TopProperty = PropertyUtils.Create(nameof(Top), typeof(double), typeof(GraphicsElement), 0.0);
        public static readonly BindableProperty WidthProperty = PropertyUtils.Create(nameof(Width), typeof(double), typeof(GraphicsElement), double.NaN);
        public static readonly BindableProperty HeightProperty = PropertyUtils.Create(nameof(Height), typeof(double), typeof(GraphicsElement), double.NaN);
        public static readonly BindableProperty RenderTransformProperty = PropertyUtils.Create(nameof(RenderTransform), typeof(Transform), typeof(GraphicsElement), null);

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