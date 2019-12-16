// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Transforms;
using XGraphics.StandardModel.Transforms;
using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public PathGeometry()
        {
            Figures = new XGraphicsCollection<PathFigure>();
        }

        public XGraphicsCollection<PathFigure> Figures { get; set; } = null;

        IEnumerable<IPathFigure> IPathGeometry.Figures => Figures;

        public FillRule FillRule { get; set; } = FillRule.EvenOdd;
    }
}