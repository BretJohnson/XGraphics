using System.Collections.Generic;
using Xamarin.Forms;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Transforms")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.XamarinForms.Geometries")]

namespace XGraphics.XamarinForms
{
    // TODO: Maybe include RenderWith later, along with reworking to have all custom renderers use same code (or at least same namespace)
    //[RenderWith(typeof(SKGLViewRenderer))]
    [ContentProperty("Children")]
    public class XGraphics : View, IXGraphics, INotifyObjectOrSubobjectChanged
    {
        public static readonly BindableProperty BackgroundProperty = PropertyUtils.Create(nameof(Background), typeof(Brush), typeof(XGraphics), null);
        public static readonly BindableProperty GraphicsRenderTransformProperty = PropertyUtils.Create(nameof(GraphicsRenderTransform), typeof(Transform), typeof(XGraphics), null);

        //private readonly bool designMode;
        private bool ignorePixelScaling;

        public event ObjectOrSubobjectChangedEventHandler Changed;

        public XGraphics()
        {
            //designMode = DesignerProperties.GetIsInDesignMode(this);
            Children = new GraphicsObjectCollection<GraphicsElement>();
            Children.Changed += OnSubobjectChanged;

            // If anything in the hierarchy changes, invalidate to trigger a redraw
            //Changed += Invalidate;
        }

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();

        IEnumerable<IGraphicsElement> IXGraphics.Children => Children;

        public GraphicsObjectCollection<GraphicsElement> Children { get; }

        IBrush IXGraphics.Background => Background;

        public Brush Background {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        ITransform? IXGraphics.GraphicsRenderTransform => GraphicsRenderTransform;
        public Transform? GraphicsRenderTransform {
            get => (Transform?)GetValue(GraphicsRenderTransformProperty);
            set => SetValue(GraphicsRenderTransformProperty, value);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Xamarin.Forms.Size(40.0, 40.0));
        }
    }
}
