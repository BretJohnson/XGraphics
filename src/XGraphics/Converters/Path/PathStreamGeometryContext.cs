// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// This class is used by the StreamGeometry class to generate an inlined,
// flattened geometry stream.
//

using System.Collections.Generic;
using System.Diagnostics;
using XGraphics.Geometries;
using XGraphics.Transforms;

namespace XGraphics.Converters.Path
{
    /// <summary>
    ///     PathStreamGeometryContext
    /// </summary>
    internal class PathStreamGeometryContext : StreamGeometryContext
    {
        internal PathStreamGeometryContext(IGeometryFactory geometryFactory)
        {
            _geometryFactory = geometryFactory;
            _pathGeometry = new PathGeometryInfo();
        }

        /// <summary>
        /// SetClosed - Sets the current closed state of the figure. 
        /// </summary>
        internal override void SetClosedState(bool isClosed)
        {
            Debug.Assert(_currentFigure != null);
            _currentFigure!.IsClosed = isClosed;
        }

        private void FinishFigure()
        {
            if (_currentFigure == null)
                return;

            FinishSegment();

            IPathFigure pathFigure = _geometryFactory.CreatePathFigure(_currentFigure.Segments, _currentFigure.StartPoint,
                isClosed: _currentFigure.IsClosed, isFilled: _currentFigure.IsFilled);
            _pathGeometry.Figures.Add(pathFigure);

            _currentFigure = null;
        }

        /// <summary>
        /// BeginFigure - Start a new figure.
        /// </summary>
        public override void BeginFigure(Point startPoint, bool isFilled, bool isClosed)
        {
            FinishFigure();
            _currentFigure = new PathFigureInfo(startPoint, isFilled: isFilled, isClosed: isClosed);
        }

        /// <summary>
        /// LineTo - append a LineTo to the current figure.
        /// </summary>
        public override void LineTo(Point point)
        {
            PrepareToAddPoints(
                        1 /*count*/,
                        MIL_SEGMENT_TYPE.MilSegmentPolyLine);

            _currentSegment!.Points.Add(point);
        }

        /// <summary>
        /// QuadraticBezierTo - append a QuadraticBezierTo to the current figure.
        /// </summary>
        public override void QuadraticBezierTo(Point point1, Point point2)
        {
            PrepareToAddPoints(
                        2 /*count*/,
                        MIL_SEGMENT_TYPE.MilSegmentPolyQuadraticBezier);

            _currentSegment!.Points.Add(point1);
            _currentSegment!.Points.Add(point2);
        }

        /// <summary>
        /// BezierTo - apply a BezierTo to the current figure.
        /// </summary>
        public override void BezierTo(Point point1, Point point2, Point point3)
        {
            PrepareToAddPoints(
                        3 /*count*/,
                        MIL_SEGMENT_TYPE.MilSegmentPolyBezier);

            _currentSegment!.Points.Add(point1);
            _currentSegment!.Points.Add(point2);
            _currentSegment!.Points.Add(point3);
        }

        /// <summary>
        /// PolyLineTo - append a PolyLineTo to the current figure.
        /// </summary>
        public override void PolyLineTo(IList<Point> points)
        {
            GenericPolyTo(points, 
                          MIL_SEGMENT_TYPE.MilSegmentPolyLine);
        }

        /// <summary>
        /// PolyQuadraticBezierTo - append a PolyQuadraticBezierTo to the current figure.
        /// </summary>
        public override void PolyQuadraticBezierTo(IList<Point> points)
        {
            GenericPolyTo(points, 
                          MIL_SEGMENT_TYPE.MilSegmentPolyQuadraticBezier);
        }

        /// <summary>
        /// PolyBezierTo - append a PolyBezierTo to the current figure.
        /// </summary>
        public override void PolyBezierTo(IList<Point> points)
        {
            GenericPolyTo(points, 
                          MIL_SEGMENT_TYPE.MilSegmentPolyBezier);
        }

        /// <summary>
        /// ArcTo - append an ArcTo to the current figure.
        /// </summary>
        public override void ArcTo(Point point, Size size, double rotationAngle, bool isLargeArc,
            SweepDirection sweepDirection)
        {
            Debug.Assert(_currentFigure != null);

            FinishSegment();

            IArcSegment segment = _geometryFactory.CreateArcSegment(point, size, rotationAngle, isLargeArc, sweepDirection);
            _currentFigure!.Segments.Add(segment);

            _currentSegment = new PathSegmentInfo(MIL_SEGMENT_TYPE.MilSegmentArc, 0);
        }

        /// <summary>
        /// GetPathGeometry - Retrieves the PathGeometry built by this Context.
        /// </summary>
        internal IPathGeometry FinishAndGetPathGeometry(FillRule fillRule, ITransform? transform)
        {
            FinishFigure();
            return _geometryFactory.CreatePathGeometry(transform, _pathGeometry.Figures, fillRule);
        }

        private void GenericPolyTo(IList<Point> points, MIL_SEGMENT_TYPE segmentType)
        {
            int count = points.Count;
            PrepareToAddPoints(count, segmentType);

            for (int i = 0; i < count; ++i)
                _currentSegment!.Points.Add(points[i]);
        }

        private void PrepareToAddPoints(int count, MIL_SEGMENT_TYPE segmentType)
        {
            Debug.Assert(_currentFigure != null);
            Debug.Assert(count != 0);

            if (_currentSegment != null && _currentSegment.Type != segmentType)
                FinishSegment();

            if (_currentSegment == null)
                _currentSegment = new PathSegmentInfo(segmentType, count);
        }

        /// <summary>
        /// FinishSegment - called to completed any outstanding Segment which may be present.
        /// </summary>
        private void FinishSegment()
        {
            // If there's no segment in progress, just return
            if (_currentSegment == null)
                return;

            Debug.Assert(_currentFigure != null);

            int count = _currentSegment.Points.Count;

            IPathSegment? segment;
            switch (_currentSegment.Type)
            {
                case MIL_SEGMENT_TYPE.MilSegmentPolyLine:
                    if (count == 1)
                        segment = _geometryFactory.CreateLineSegment(_currentSegment.Points[0]);
                    else
                        segment = _geometryFactory.CreatePolyLineSegment(_currentSegment.Points.ToArray());
                    break;
                case MIL_SEGMENT_TYPE.MilSegmentPolyBezier:
                    if (count == 3)
                        segment = _geometryFactory.CreateBezierSegment(_currentSegment.Points[0],
                            _currentSegment.Points[1], _currentSegment.Points[2]);
                    else
                    {
                        Debug.Assert(count % 3 == 0);
                        segment = _geometryFactory.CreatePolyBezierSegment(_currentSegment.Points.ToArray());
                    }
                    break;
                case MIL_SEGMENT_TYPE.MilSegmentPolyQuadraticBezier:
                    if (count == 2)
                        segment = _geometryFactory.CreateQuadraticBezierSegment(_currentSegment.Points[0],
                            _currentSegment.Points[1]);
                    else
                    {
                        Debug.Assert(count % 2 == 0);
                        segment = _geometryFactory.CreatePolyQuadraticBezierSegment(_currentSegment.Points.ToArray());
                    }
                    break;
                default:
                    segment = null;
                    Debug.Assert(false);
                    break;
            }

            if (segment != null)
                _currentFigure!.Segments.Add(segment);

            _currentSegment = null;
        }

        private readonly IGeometryFactory _geometryFactory;

        private readonly PathGeometryInfo _pathGeometry;
        private PathFigureInfo? _currentFigure;
        private PathSegmentInfo? _currentSegment;


        private enum MIL_SEGMENT_TYPE
        {
            MilSegmentNone,
            MilSegmentLine,
            MilSegmentBezier,
            MilSegmentQuadraticBezier,
            MilSegmentArc,
            MilSegmentPolyLine,
            MilSegmentPolyBezier,
            MilSegmentPolyQuadraticBezier,

            MIL_SEGMENT_TYPE_FORCE_DWORD = unchecked((int)0xffffffff)
        };

        private class PathGeometryInfo
        {
            public List<IPathFigure> Figures { get; }

            public PathGeometryInfo()
            {
                Figures = new List<IPathFigure>();
            }
        }

        private class PathFigureInfo
        {
            public List<IPathSegment> Segments { get; }
            public Point StartPoint { get; }
            public bool IsFilled { get; }
            public bool IsClosed { get; set; }

            public PathFigureInfo(in Point startPoint, bool isFilled, bool isClosed)
            {
                Segments = new List<IPathSegment>();
                StartPoint = startPoint;
                IsFilled = isFilled;
                IsClosed = isClosed;
            }
        }

        private class PathSegmentInfo
        {
            public List<Point> Points { get; }
            public MIL_SEGMENT_TYPE Type { get; }

            public PathSegmentInfo(MIL_SEGMENT_TYPE type, int count)
            {
                Type = type;
                Points = new List<Point>(count);
            }
        }
    }
}
