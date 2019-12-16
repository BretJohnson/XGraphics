// This file is generated from IPath.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using XGraphics.StandardModel.Geometries;
using XGraphics.Shapes;

namespace XGraphics.StandardModel.Shapes
{
    public class Path : Shape, IPath
    {
        public Geometry Data { get; set; } = null;

        IGeometry IPath.Data => Data;
    }
}