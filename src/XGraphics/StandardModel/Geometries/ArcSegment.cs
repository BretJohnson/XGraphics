// This file is generated from IArcSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public Point Point { get; set; } = Point.Default;

        public Size Size { get; set; } = Size.Default;

        public double RotationAngle { get; set; } = 0.0;

        public bool IsLargeArc { get; set; } = false;

        public SweepDirection SweepDirection { get; set; } = SweepDirection.Counterclockwise;
    }
}