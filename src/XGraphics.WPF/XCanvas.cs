using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XGraphics.Brushes;
using XGraphics.Transforms;
using Transform = XGraphics.WPF.Transforms.Transform;


[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.WPF")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.WPF.Brushes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.WPF.Geometries")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.WPF.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xgraphics/2019", "XGraphics.WPF.Transforms")]

namespace XGraphics.WPF
{
    struct SizeInPixels
    {
        public static SizeInPixels Empty = new SizeInPixels(-1, -1);

        public int Width { get; }
        public int Height { get; }

        public SizeInPixels(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    [ContentProperty("Children")]
    public class XCanvas : FrameworkElement, IXCanvas, INotifyObjectOrSubobjectChanged
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(XGraphicsCollection<GraphicsElement>), typeof(XCanvas), null);
        public static readonly DependencyProperty BackgroundProperty = PropertyUtils.Create(nameof(Background), typeof(Brushes.Brush), typeof(XCanvas), null);
        public static readonly DependencyProperty GraphicsRenderTransformProperty = PropertyUtils.Create(nameof(GraphicsRenderTransform), typeof(Transform), typeof(XCanvas), null);

        private readonly bool _designMode;
        private WriteableBitmap? _bitmap;
        private bool _ignorePixelScaling;

        public event ObjectOrSubobjectChangedEventHandler Changed;

        public XCanvas()
        {
            _designMode = DesignerProperties.GetIsInDesignMode(this);
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
        public Brushes.Brush Background {
            get => (Brushes.Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public bool IgnorePixelScaling
		{
			get => _ignorePixelScaling;
            set
			{
				_ignorePixelScaling = value;
				InvalidateVisual();
			}
		}

        ITransform? IXCanvas.GraphicsRenderTransform => GraphicsRenderTransform;
        public Transform? GraphicsRenderTransform {
            get => (Transform?)GetValue(GraphicsRenderTransformProperty);
            set => SetValue(GraphicsRenderTransformProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (_designMode)
                return;

            if (Visibility != Visibility.Visible)
                return;

            SizeInPixels size = ComputeSize(out double scaleX, out double scaleY);
            if (size.Width <= 0 || size.Height <= 0)
                return;

            // reset the bitmap if the size has changed
            if (_bitmap == null || size.Width != _bitmap.PixelWidth || size.Height != _bitmap.PixelHeight)
                _bitmap = new WriteableBitmap(size.Width, size.Height, 96 * scaleX, 96 * scaleY, PixelFormats.Pbgra32, null);

            // draw on the bitmap
            _bitmap.Lock();

            XGraphicsRenderer? graphicsRenderer = XGraphicsRenderer.DefaultRenderer;
            if (graphicsRenderer == null)
                throw new InvalidOperationException("GraphicsRenderer.DefaultRenderer must be initialized before attempting to render XGraphics");

            graphicsRenderer.RenderToBuffer(this, _bitmap.BackBuffer, size.Width, size.Height, _bitmap.BackBufferStride);

            // draw the bitmap to the screen
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, size.Width, size.Height));
            _bitmap.Unlock();
            drawingContext.DrawImage(_bitmap, new Rect(0, 0, ActualWidth, ActualHeight));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        private SizeInPixels ComputeSize(out double scaleX, out double scaleY)
        {
            scaleX = 1.0;
            scaleY = 1.0;

            double w = ActualWidth;
            double h = ActualHeight;

            if (!IsPositive(w) || !IsPositive(h))
                return SizeInPixels.Empty;

            if (IgnorePixelScaling)
                return new SizeInPixels((int)w, (int)h);

            Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            scaleX = m.M11;
            scaleY = m.M22;
            return new SizeInPixels((int)(w * scaleX), (int)(h * scaleY));

            bool IsPositive(double value)
            {
                return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
            }
        }

        public void Invalidate()
        {
            InvalidateVisual();
        }

#if false
        protected IGraphicsCanvasRenderer GetGraphicsCanvasRenderer()
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent is IGraphicsCanvasRenderer renderer)
                    return renderer;
                parent = parent.Parent;
            }
            return null;
        }

        protected static void InvalidateGraphicsCanvas(GraphicsElement element)
        {
            element.GetGraphicsCanvasRenderer()?.Invalidate();
        }

        protected static void OnGraphicsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is GraphicsElement element)
            {
                InvalidateGraphicsCanvas(element);
            }
        }
#endif
    }
}
