// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Brushes;
using XGraphics.XamarinForms.Brushes;
using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    [ContentProperty("Children")]
    public class Canvas : GraphicsElement, ICanvas
    {
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(XGraphicsCollection<GraphicsElement>), typeof(Canvas), null);
        public static readonly BindableProperty BackgroundProperty = PropertyUtils.Create(nameof(Background), typeof(Brush), typeof(Canvas), null);

        public Canvas()
        {
            Children = new XGraphicsCollection<GraphicsElement>();
        }

        public XGraphicsCollection<GraphicsElement> Children
        {
            get => (XGraphicsCollection<GraphicsElement>)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }
        IEnumerable<IGraphicsElement> ICanvas.Children => Children;

        public Brush? Background
        {
            get => (Brush?)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        IBrush? ICanvas.Background => Background;
    }
}
