// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public Point Point1 { get; set; } = Point.Default;

        public Point Point2 { get; set; } = Point.Default;
    }
}