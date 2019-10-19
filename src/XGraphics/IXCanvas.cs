using System.Collections.Generic;
using XGraphics.Brushes;
using XGraphics.Transforms;

namespace XGraphics
{
    public interface IXCanvas
    {
        IEnumerable<IGraphicsElement> Children { get; }

        IBrush? Background { get; }

        [ModelDefaultValue(null)]
        ITransform? GraphicsRenderTransform { get; }
    }
}
