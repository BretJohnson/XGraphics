using System.Collections.Generic;
using Xamarin.Forms;
using XGraphics.Brushes;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Brushes;
using XGraphics.XamarinForms.Transforms;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Brushes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Geometries")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Transforms")]

namespace XGraphics.XamarinForms
{
    // TODO: Maybe include RenderWith later, along with reworking to have all custom renderers use same code (or at least same namespace)
    //[RenderWith(typeof(SKGLViewRenderer))]
    [ContentProperty("Children")]
    public class XCanvas : View, IXCanvas, INotifyObjectOrSubobjectChanged
    {
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(XGraphicsCollection<GraphicsElement>), typeof(Canvas), null);
        public static readonly BindableProperty BackgroundProperty = PropertyUtils.Create(nameof(Background), typeof(Brush), typeof(XCanvas), null);
        public static readonly BindableProperty GraphicsRenderTransformProperty = PropertyUtils.Create(nameof(GraphicsRenderTransform), typeof(Transform), typeof(XCanvas), null);

        //private readonly bool designMode;
        private bool _ignorePixelScaling;

        public event ObjectOrSubobjectChangedEventHandler Changed;

        public XCanvas()
        {
            //designMode = DesignerProperties.GetIsInDesignMode(this);
            Children = new XGraphicsCollection<GraphicsElement>();

            // If anything in the hierarchy changes, invalidate to trigger a redraw
            Changed += Invalidate;
        }

        public void NotifySinceObjectChanged() => Changed?.Invoke();

        public void NotifySinceSubobjectChanged() => Changed?.Invoke();

        public XGraphicsCollection<GraphicsElement> Children
        {
            get => (XGraphicsCollection<GraphicsElement>)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }
        IEnumerable<IGraphicsElement> IXCanvas.Children => Children;

        IBrush? IXCanvas.Background => Background;

        public Brush Background {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        ITransform? IXCanvas.GraphicsRenderTransform => GraphicsRenderTransform;
        public Transform? GraphicsRenderTransform {
            get => (Transform?)GetValue(GraphicsRenderTransformProperty);
            set => SetValue(GraphicsRenderTransformProperty, value);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Xamarin.Forms.Size(40.0, 40.0));
        }

        public void Invalidate()
        {
            // TODO: Implement this
        }
    }
}
