using System.Collections.Generic;
using XGraphics.Brushes;
using XGraphics.StandardModel.Brushes;
using XGraphics.StandardModel.Transforms;
using XGraphics.Transforms;

namespace XGraphics.StandardModel
{
    public class XCanvas : IXCanvas
    {
        public XCanvas()
        {
            Children = new List<GraphicsElement>();
        }

        IEnumerable<IGraphicsElement> IXCanvas.Children => Children;
        public List<GraphicsElement> Children { get; }

        IBrush? IXCanvas.Background => Background;
        public Brush? Background { get; set; } = null;

        ITransform? IXCanvas.GraphicsRenderTransform => GraphicsRenderTransform;
        public Transform? GraphicsRenderTransform { get; set; } = null;
    }
}
