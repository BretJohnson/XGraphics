// This file is generated from IGeometry.cs. Update the source file to change its contents.

using XGraphics.Transforms;
using XGraphics.StandardModel.Transforms;
using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class Geometry : ObjectWithCascadingNotifications, IGeometry
    {
        public double StandardFlatteningTolerance { get; set; } = 0.25;

        public Transform Transform { get; set; } = null;

        ITransform IGeometry.Transform => Transform;
    }
}