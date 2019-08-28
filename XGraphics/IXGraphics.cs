using System.Collections.Generic;
using XGraphics.Transforms;

namespace XGraphics
{
    public interface IXGraphics
    {
        IEnumerable<IGraphicsElement> Children { get; }

        IBrush? Background { get; }

        [ModelDefaultValue(null)]
        ITransform? GraphicsRenderTransform { get; }
    }
}
