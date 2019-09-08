using System.Collections.Generic;
using XGraphics.Transforms;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPathGeometry : IGeometry
    {
        IEnumerable<IPathFigure> Figures { get; }

        [ModelDefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; }
    }
}
