using System.Collections.Generic;

namespace XGraphics
{
    [GraphicsModelObject]
    public interface ICanvas : IGraphicsElement
    {
        IEnumerable<IGraphicsElement> Children { get;  }

        [ModelDefaultValue(null)]
        IBrush? Background { get; }
    }
}
