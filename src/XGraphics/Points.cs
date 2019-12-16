using System;
using System.Collections.Generic;

namespace XGraphics
{
    public struct Points
    {
        private readonly Point[] _points;

        public static readonly Points Default = new Points(Array.Empty<Point>());

        public Points(Point[] points)
        {
            _points = points;
        }

        public Points(List<Point> points)
        {
            _points = points.ToArray();
        }

        public int Length => _points.Length;

        public Point this[int index] => _points[index];
    }
}
