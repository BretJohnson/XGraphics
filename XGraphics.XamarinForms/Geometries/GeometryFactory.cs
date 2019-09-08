using System;
using System.Collections.Generic;
using XGraphics.Converters;
using XGraphics.Geometries;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;

namespace XGraphics.XamarinForms.Geometries
{
    public class GeometryFactory : IGeometryFactory
    {
        public static GeometryFactory Instance = new GeometryFactory();

        public ILineSegment CreateLineSegment(in Point point) =>
            new LineSegment()
            {
                Point = new Wrapper.Point(point)
            };

        public IPolyLineSegment CreatePolyLineSegment(Point[] points)
        {
            var polyLineSegment = new PolyLineSegment();
            polyLineSegment.Points = points;
            return polyLineSegment;
        }

        public IBezierSegment CreateBezierSegment(in Point point1, in Point point2, in Point point3) =>
            new BezierSegment()
            {
                Point1 = new Wrapper.Point(point1),
                Point2 = new Wrapper.Point(point2),
                Point3 = new Wrapper.Point(point3)
            };

        public IPolyBezierSegment CreatePolyBezierSegment(Point[] points)
        {
            var polyBezierSegment = new PolyBezierSegment();
            polyBezierSegment.Points = points;
            return polyBezierSegment;
        }

        public IQuadraticBezierSegment CreateQuadraticBezierSegment(in Point point1, in Point point2) =>
            new QuadraticBezierSegment()
            {
                Point1 = new Wrapper.Point(point1),
                Point2 = new Wrapper.Point(point2)
            };

        public IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(Point[] points)
        {
            var polyQuadraticBezierSegment = new PolyQuadraticBezierSegment();
            polyQuadraticBezierSegment.Points = points;
            return polyQuadraticBezierSegment;
        }

        public IArcSegment CreateArcSegment(in Point point, in Size size, double rotationAngle, bool isLargeArc,
            SweepDirection sweepDirection) =>
            new ArcSegment()
            {
                Point = new Wrapper.Point(point),
                Size = new Wrapper.Size(size),
                RotationAngle = rotationAngle,
                IsLargeArc = isLargeArc,
                SweepDirection = sweepDirection
            };

        public IPathGeometry CreatePathGeometry(ITransform? transformInterface, IEnumerable<IPathFigure> figures, FillRule fillRule)
        {
            var pathGeometry = new PathGeometry()
            {
                FillRule = fillRule
            };

            if (transformInterface != null)
            {
                if (!(transformInterface is Transform transform))
                    throw new InvalidOperationException($"Transforms should all be of type {nameof(Transform)}");

                pathGeometry.Transform = transform;
            }

            GraphicsObjectCollection<PathFigure> destinationFigures = pathGeometry.Figures;
            foreach (IPathFigure pathFigureInterface in figures)
            {
                if (! (pathFigureInterface is PathFigure pathFigure))
                    throw new InvalidOperationException($"{nameof(CreatePathGeometry)} figures should all be of type {nameof(PathFigure)}");

                destinationFigures.Add(pathFigure);
            }

            return pathGeometry;
        }

        public IPathFigure CreatePathFigure(IEnumerable<IPathSegment> segments, Point startPoint, bool isClosed, bool isFilled)
        {
            var pathFigure = new PathFigure()
            {
                StartPoint = new Wrapper.Point(startPoint),
                IsClosed = isClosed,
                IsFilled = isFilled
            };

            GraphicsObjectCollection<PathSegment> destinationSegments = pathFigure.Segments;
            foreach (IPathSegment pathSegmentInterface in segments)
            {
                if (!(pathSegmentInterface is PathSegment pathSegment))
                    throw new InvalidOperationException($"{nameof(CreatePathFigure)} segments should all be of type {nameof(PathSegment)}");

                destinationSegments.Add(pathSegment);
            }

            return pathFigure;
        }
    }
}
