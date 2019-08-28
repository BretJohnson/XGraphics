using System.Collections.Generic;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IPathFigure
    {
        IEnumerable<IPathSegment> Segments { get; }

        Point StartPoint { get; }

        [ModelDefaultValue(false)]
        bool IsClosed { get; }

        [ModelDefaultValue(true)]
        bool IsFilled { get; }
    }
}
