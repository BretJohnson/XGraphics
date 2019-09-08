using System.Collections.Generic;
using XGraphics.Transforms;

namespace XGraphics.Geometries
{
    public interface IGeometryFactory
    {
        ILineSegment CreateLineSegment(in Point point);
        IPolyLineSegment CreatePolyLineSegment(Point[] points);
        IBezierSegment CreateBezierSegment(in Point point1, in Point point2, in Point point3);
        IPolyBezierSegment CreatePolyBezierSegment(Point[] points);
        IQuadraticBezierSegment CreateQuadraticBezierSegment(in Point point1, in Point point2);
        IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(Point[] points);
        IArcSegment CreateArcSegment(in Point point, in Size size, double rotationAngle, bool isLargeArc,
            SweepDirection sweepDirection);

        IPathGeometry CreatePathGeometry(ITransform? transform, IEnumerable<IPathFigure> figures, FillRule fillRule);
        IPathFigure CreatePathFigure(IEnumerable<IPathSegment> segments, Point startPoint, bool isClosed, bool isFilled);
    }
}
