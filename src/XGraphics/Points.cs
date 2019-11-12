using System;

namespace XGraphics
{
    public struct Points
    {
        public static readonly Points Default = new Points(Array.Empty<Point>());

        public Points(Point[] value)
        {
            Value = value;
        }

        public Point[] Value { get; }
    }
}
