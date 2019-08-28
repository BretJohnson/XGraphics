using System;
using System.Collections.Generic;
using SkiaSharp;
using XGraphics.Geometries;
using XGraphics.Shapes;
using XGraphics.Transforms;

namespace XGraphics.Renderer.Skia
{
    public class SkiaPainter
    {
        private readonly SKSurface surface;
        private readonly SKCanvas skCanvas;

        public SkiaPainter(SKSurface surface)
        {
            this.surface = surface;
            skCanvas = surface.Canvas;
        }

        public void Paint(IXGraphics xGraphics)
        {
            IBrush? background = xGraphics.Background;
            if (background is ISolidColorBrush solidColorBrush)
                skCanvas.Clear(ToSkiaColor(solidColorBrush.Color));
            else skCanvas.Clear(SKColors.Transparent);

            ITransform? xGraphicsRenderTransform = xGraphics.GraphicsRenderTransform;
            if (xGraphicsRenderTransform != null)
                ApplyTransform(xGraphicsRenderTransform, xGraphics);

            foreach (IGraphicsElement graphicsElement in xGraphics.Children)
            {
                ITransform? renderTransform = graphicsElement.RenderTransform;

                if (renderTransform != null)
                {
                    skCanvas.Save();
                    ApplyTransform(renderTransform, graphicsElement);
                }

                if (graphicsElement is IPath path)
                    PaintPath(path);
                else if (graphicsElement is IShape shape)
                {
                    SKPath skiaPath = NonPathShapeToSkiaPath(shape);
                    PaintSkiaPath(skiaPath, shape);
                }
                else throw new InvalidOperationException($"Unknown graphics element type {graphicsElement.GetType()}");

                if (renderTransform != null)
                    skCanvas.Restore();
            }
        }

        private void PaintPath(IPath path)
        {
            IGeometry geometry = path.Data;

            ITransform? transform = geometry.Transform;

            if (transform != null)
                throw new InvalidOperationException("Transforms on geometries aren't currently supported");

            if (geometry is IPathGeometry pathGeometry)
            {
                List<SkiaPathFigure> skiaPathFigures = PathGeometryToSkiaPathFigures(pathGeometry);

                foreach (SkiaPathFigure skiaPathFigure in skiaPathFigures)
                    PaintSkiaPath(skiaPathFigure.Path, path, skiaPathFigure.IsFilled);
            }
            else throw new InvalidOperationException($"Geometry type {geometry.GetType()} isn't currently supported; only IPathGeometry is supported currently");
        }

        private SKPath NonPathShapeToSkiaPath(IShape shape)
        {
            SKPath skPath = new SKPath();
            if (shape is IRectangle rectangle)
            {
                SKRect skRect = SKRect.Create((float) rectangle.Left, (float) rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                if (rectangle.RadiusX > 0 || rectangle.RadiusY > 0)
                    skPath.AddRoundRect(skRect, (float) rectangle.RadiusX, (float)rectangle.RadiusY);
                else
                    skPath.AddRect(skRect);
            }
            else if (shape is ILine line)
            {
                skPath.MoveTo((float)line.X1, (float)line.Y1);
                skPath.LineTo((float)line.X2, (float)line.Y2);
            }
            else if (shape is IEllipse ellipse)
            {
                SKRect skRect = SKRect.Create((float)ellipse.Left, (float)ellipse.Top, (float)ellipse.Width, (float)ellipse.Height);
			    skPath.AddOval(skRect);
            }
            return skPath;
        }

        private List<SkiaPathFigure> PathGeometryToSkiaPathFigures(IPathGeometry pathGeometry)
        {
            SKPathFillType fillType = pathGeometry.FillRule switch
                {
                FillRule.EvenOdd => SKPathFillType.EvenOdd,
                FillRule.Nonzero => SKPathFillType.Winding,
                _ => throw new InvalidOperationException($"Unknown fillRule value {pathGeometry.FillRule}")
                };

            List<SkiaPathFigure> skiaPathFigures = new List<SkiaPathFigure>();
            // TODO: Decide how (or if) to support geometry.StandardFlatteningTolerance

            foreach (IPathFigure pathFigure in pathGeometry.Figures)
            {
                Point startPoint = pathFigure.StartPoint;

                SKPath skPath = new SKPath();

                skPath.FillType = fillType;
                skPath.MoveTo((float)startPoint.X, (float)startPoint.Y);

                foreach (IPathSegment pathSegment in pathFigure.Segments)
                    AddPathSegmentToSkiaPath(skPath, pathSegment);

                if (pathFigure.IsClosed)
                    skPath.LineTo((float)startPoint.X, (float)startPoint.Y);

                skiaPathFigures.Add(new SkiaPathFigure(skPath, pathFigure.IsFilled));
            }

            return skiaPathFigures;
        }
        private void AddPathSegmentToSkiaPath(SKPath skPath, IPathSegment pathSegment)
        {
            if (pathSegment is IBezierSegment bezierSegment)
                skPath.CubicTo(
                    (float)bezierSegment.Point1.X, (float)bezierSegment.Point1.Y,
                    (float)bezierSegment.Point2.X, (float)bezierSegment.Point2.Y,
                    (float)bezierSegment.Point3.X, (float)bezierSegment.Point3.Y);
            else if (pathSegment is ILineSegment lineSegment)
                skPath.LineTo((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
            else if (pathSegment is IQuadraticBezierSegment quadraticBezierSegment)
                skPath.QuadTo(
                    (float) quadraticBezierSegment.Point1.X, (float) quadraticBezierSegment.Point1.Y,
                    (float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y);
            else if (pathSegment is IPolyLineSegment polyLineSegment)
            {
                var skiaPoints = new List<SKPoint>();
                AddSkiaPoints(polyLineSegment.Points, skiaPoints);
                skPath.AddPoly(skiaPoints.ToArray());
            }
            else throw new InvalidOperationException($"IPathSegment type {pathSegment.GetType()} not yet implemented");
        }

        private void ApplyTransform(ITransform transform, IGraphicsElement graphicsElement)
        {
            if (transform is IRotateTransform rotateTransform)
            {
                double centerX = graphicsElement.Left + rotateTransform.CenterX;
                double centerY = graphicsElement.Top + rotateTransform.CenterY;

                skCanvas.RotateDegrees((float) rotateTransform.Angle, (float) centerX, (float) centerY);
            }
            else if (transform is IScaleTransform scaleTransform)
            {
                double centerX = graphicsElement.Left + scaleTransform.CenterX;
                double centerY = graphicsElement.Top + scaleTransform.CenterY;

                skCanvas.Scale((float) scaleTransform.ScaleX, (float) scaleTransform.ScaleY, (float) centerX, (float) centerY);
            }
            else if (transform is ITranslateTransform translateTransform)
            {
                skCanvas.Translate((float) translateTransform.X, (float) translateTransform.Y);
            }
            else if (transform is ITransformGroup transformGroup)
            {
                foreach (ITransform childTransform in transformGroup.Children)
                    ApplyTransform(childTransform, graphicsElement);
            }
            else throw new InvalidOperationException($"Unknown transform type {transform.GetType()}");
        }

        private void ApplyTransform(ITransform transform, IXGraphics xGraphics)
        {
            if (transform is IRotateTransform rotateTransform)
                skCanvas.RotateDegrees((float) rotateTransform.Angle, (float) rotateTransform.CenterX,
                    (float) rotateTransform.CenterY);
            else if (transform is IScaleTransform scaleTransform)
                skCanvas.Scale((float) scaleTransform.ScaleX, (float) scaleTransform.ScaleY,
                    (float) scaleTransform.CenterX, (float) scaleTransform.CenterY);
            else if (transform is ITranslateTransform translateTransform)
                skCanvas.Translate((float) translateTransform.X, (float) translateTransform.Y);
            else if (transform is ITransformGroup transformGroup)
            {
                foreach (ITransform childTransform in transformGroup.Children)
                    ApplyTransform(childTransform, xGraphics);
            }
            else throw new InvalidOperationException($"Unknown transform type {transform.GetType()}");
        }

        private void PaintSkiaPath(SKPath skiaPath, IShape shape, bool allowFill = true)
        {
            var fill = shape.Fill;
            if (fill != null && allowFill)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Fill, IsAntialias = true };
                InitSkiaPaintForBrush(paint, fill);
                skCanvas.DrawPath(skiaPath, paint);
            }

            var stroke = shape.Stroke;
            if (stroke != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Stroke, IsAntialias = true };
                InitSkiaPaintForBrush(paint, stroke);
                paint.StrokeWidth = (int)shape.StrokeThickness;

                skCanvas.DrawPath(skiaPath, paint);
            }
        }

        public static void InitSkiaPaintForBrush(SKPaint paint, IBrush brush)
        {
            if (brush is ISolidColorBrush solidColorBrush)
                paint.Color = ToSkiaColor(solidColorBrush.Color);
            else throw new InvalidOperationException($"Brush type {brush.GetType()} isn't currently supported");
        }

        public static SKColor ToSkiaColor(Color color) =>
            new SKColor(color.R, color.G, color.B, color.A);

        public static SKPoint ToSkiaPoint(Point point) =>
            new SKPoint((float) point.X, (float) point.Y);

        public static void AddSkiaPoints(IEnumerable<Point> points, List<SKPoint> skiaPoints)
        {
            foreach (Point point in points)
                skiaPoints.Add(new SKPoint((float)point.X, (float)point.Y));
        }
    }
}
