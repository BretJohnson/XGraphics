using System.Collections.Generic;
using System.Linq;

namespace XGraphics.Transforms
{
    [GraphicsModelObject]
    public interface ITransformGroup : ITransform
    {
        IEnumerable<ITransform> Children { get; }
    }
}
