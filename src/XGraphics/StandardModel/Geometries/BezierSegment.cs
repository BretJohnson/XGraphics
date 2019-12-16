// This file is generated from IBezierSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class BezierSegment : PathSegment, IBezierSegment
    {
        public Point Point1 { get; set; } = Point.Default;

        public Point Point2 { get; set; } = Point.Default;

        public Point Point3 { get; set; } = Point.Default;
    }
}