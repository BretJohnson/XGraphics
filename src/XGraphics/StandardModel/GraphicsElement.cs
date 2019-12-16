// This file is generated from IGraphicsElement.cs. Update the source file to change its contents.

using XGraphics.Shapes;
using XGraphics.StandardModel.Shapes;
using XGraphics.Transforms;
using XGraphics.StandardModel.Transforms;
using XGraphics;

namespace XGraphics.StandardModel
{
    public class GraphicsElement : ObjectWithCascadingNotifications, IGraphicsElement
    {
        public double Left { get; set; } = 0.0;

        public double Top { get; set; } = 0.0;

        public Transform? RenderTransform { get; set; } = null;

        ITransform? IGraphicsElement.RenderTransform => RenderTransform;
    }
}