using System.Collections.Generic;
using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    [ContentProperty("Children")]
    public class Canvas : GraphicsElement, ICanvas
    {
        public static readonly BindableProperty BackgroundProperty = PropertyUtils.Create(nameof(Background), typeof(Brush), typeof(Canvas), null);

        public Canvas()
        {
            Children = new GraphicsObjectCollection<GraphicsElement>();
            Children.Changed += OnSubobjectChanged;
        }

        IEnumerable<IGraphicsElement> ICanvas.Children => Children;
        public GraphicsObjectCollection<GraphicsElement> Children
        {
            get;
        }

        IBrush? ICanvas.Background => Background;
        public Brush? Background
        {
            get => (Brush? )GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
    }
}