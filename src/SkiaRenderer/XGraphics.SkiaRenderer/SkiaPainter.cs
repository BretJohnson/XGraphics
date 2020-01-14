using System;
using System.Collections.Generic;
using SkiaSharp;
using XGraphics.Brushes;
using XGraphics.Geometries;
using XGraphics.ImageLoading.Work;
using XGraphics.Shapes;
using XGraphics.Transforms;

namespace XGraphics.SkiaRenderer
{
    public class SkiaPainter
    {
        private readonly SKSurface surface;
        private readonly SKCanvas skCanvas;
        private readonly IImageLoader _imageLoader;

        public SkiaPainter(SKSurface surface, IImageLoader imageLoader)
        {
            this.surface = surface;
            skCanvas = surface.Canvas;
            _imageLoader = imageLoader;
        }

        public void Paint(IXCanvas xCanvas)
        {
            IBrush? background = xCanvas.Background;
            if (background is ISolidColorBrush solidColorBrush)
                skCanvas.Clear(ToSkiaColor(solidColorBrush.Color));
            else skCanvas.Clear(SKColors.Transparent);

            ITransform? xGraphicsRenderTransform = xCanvas.GraphicsRenderTransform;
            if (xGraphicsRenderTransform != null)
                ApplyTransform(xGraphicsRenderTransform, xCanvas);

            PaintGraphicsElements(xCanvas.Children);
        }

        private void PaintGraphicsElements(IEnumerable<IGraphicsElement> graphicsElements)
        {
            foreach (IGraphicsElement graphicsElement in graphicsElements)
            {
                ITransform? renderTransform = graphicsElement.RenderTransform;

                if (renderTransform != null)
                {
                    skCanvas.Save();
                    ApplyTransform(renderTransform, graphicsElement);
                }

                if (graphicsElement is IPath path)
                    PaintPath(path);
                else if (graphicsElement is ICanvas canvas)
                {
                    // TODO: Handle canvas offset / transform
                    PaintGraphicsElements(canvas.Children);
                }
                else if (graphicsElement is IShape shape)
                {
                    SKPath skiaPath = NonPathShapeToSkiaPath(shape);
                    FillSkiaPath(skiaPath, shape);
                    StrokeSkiaPath(skiaPath, shape);
                }
                else if (graphicsElement is IImage image)
                {
                    IImageSource imageSource = image.Source;

                    if (imageSource is ILoadableImageSource loadableImageSource)
                    {
                        if (true /* needs loading */)
                        {
                            _imageLoader.LoadImage(loadableImageSource, null);
                        }
                    }
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
                SKPath skiaPathForFill = PathGeometryToSkiaPath(pathGeometry, onlyIncludeFilledFigures:true);
                SKPath skiaPathForStroke = PathGeometryToSkiaPath(pathGeometry);

                FillSkiaPath(skiaPathForFill, path);
                StrokeSkiaPath(skiaPathForStroke, path);
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
            else if (shape is IPolygon polygon)
            {
                skPath.FillType = FillRuleToSkiaPathFillType(polygon.FillRule);
                skPath.AddPoly(PointsToSkiaPoints(polygon.Points), close: true);
            }
            else if (shape is IPolyline polyline)
            {
                skPath.FillType = FillRuleToSkiaPathFillType(polyline.FillRule);
                skPath.AddPoly(PointsToSkiaPoints(polyline.Points), close: false);
            }
            return skPath;
        }

        private SKPath PathGeometryToSkiaPath(IPathGeometry pathGeometry, bool onlyIncludeFilledFigures = false)
        {
            // TODO: Decide how (or if) to support geometry.StandardFlatteningTolerance

            SKPath skPath = new SKPath();
            skPath.FillType = FillRuleToSkiaPathFillType(pathGeometry.FillRule);

            foreach (IPathFigure pathFigure in pathGeometry.Figures)
            {
                if (onlyIncludeFilledFigures && !pathFigure.IsFilled)
                    continue;

                Point startPoint = pathFigure.StartPoint;

                skPath.MoveTo((float)startPoint.X, (float)startPoint.Y);

                foreach (IPathSegment pathSegment in pathFigure.Segments)
                    AddPathSegmentToSkiaPath(skPath, pathSegment);

                if (pathFigure.IsClosed)
                    skPath.Close();
            }

            return skPath;
        }

        private static SKPathFillType FillRuleToSkiaPathFillType(FillRule fillRule)
        {
            return fillRule switch
            {
                FillRule.EvenOdd => SKPathFillType.EvenOdd,
                FillRule.Nonzero => SKPathFillType.Winding,
                _ => throw new InvalidOperationException($"Unknown fillRule value {fillRule}")
            };
        }

        private SKPoint[] PointsToSkiaPoints(Points points)
        {
            int length = points.Length;
            SKPoint[] skiaPoints = new SKPoint[length];
            for (int i = 0; i < length; i++)
                skiaPoints[i] = new SKPoint((float)points[i].X, (float)points[i].Y);

            return skiaPoints;
        }

        private void AddPathSegmentToSkiaPath(SKPath skPath, IPathSegment pathSegment)
        {
            if (pathSegment is IBezierSegment bezierSegment)
                skPath.CubicTo(
                    (float)bezierSegment.Point1.X, (float)bezierSegment.Point1.Y,
                    (float)bezierSegment.Point2.X, (float)bezierSegment.Point2.Y,
                    (float)bezierSegment.Point3.X, (float)bezierSegment.Point3.Y);
            else if (pathSegment is IPolyBezierSegment polyBezierSegment)
            {
                Points points = polyBezierSegment.Points;
                int length = points.Length;

                if (length % 3 != 0)
                    throw new InvalidOperationException($"IPolyBezierSegment contains {length} points, which isn't a multiple of 3");

                for (int i = 0; i < length;)
                {
                    Point point1 = points[i + 0];
                    Point point2 = points[i + 1];
                    Point point3 = points[i + 2];

                    skPath.CubicTo(
                        (float)point1.X, (float)point1.Y,
                        (float)point2.X, (float)point2.Y,
                        (float)point3.X, (float)point3.Y);

                    i += 3;
                }
            }
            else if (pathSegment is ILineSegment lineSegment)
                skPath.LineTo((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
            else if (pathSegment is IQuadraticBezierSegment quadraticBezierSegment)
                skPath.QuadTo(
                    (float) quadraticBezierSegment.Point1.X, (float) quadraticBezierSegment.Point1.Y,
                    (float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y);
            else if (pathSegment is IPolyLineSegment polyLineSegment)
            {
                Points points = polyLineSegment.Points;
                int length = points.Length;

                for (int i = 0; i < length; i++)
                {
                    Point point = points[i];
                    skPath.LineTo((float)point.X, (float)point.Y);
                }
            }
            else if (pathSegment is IArcSegment arcSegment)
            {
                SKPathDirection skiaPathDirection = arcSegment.SweepDirection switch
                {
                    SweepDirection.Clockwise => SKPathDirection.Clockwise,
                    SweepDirection.Counterclockwise => SKPathDirection.CounterClockwise,
                    _ => throw new InvalidOperationException($"Unknown SweepDirection value {arcSegment.SweepDirection}")
                };

                SKPathArcSize skiaPathArcSize = arcSegment.IsLargeArc ? SKPathArcSize.Large : SKPathArcSize.Small;

                skPath.ArcTo((float)arcSegment.Size.Width, (float)arcSegment.Size.Height,
                    (float)arcSegment.RotationAngle,
                    skiaPathArcSize, skiaPathDirection, (float)arcSegment.Point.X, (float)arcSegment.Point.Y);
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

        private void ApplyTransform(ITransform transform, IXCanvas xCanvas)
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
                    ApplyTransform(childTransform, xCanvas);
            }
            else throw new InvalidOperationException($"Unknown transform type {transform.GetType()}");
        }

        private void FillSkiaPath(SKPath skiaPath, IShape shape)
        {
            IBrush? fill = shape.Fill;
            if (fill != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Fill, IsAntialias = true };
                InitSkiaPaintForBrush(paint, fill, shape);
                skCanvas.DrawPath(skiaPath, paint);
            }
        }

        private void StrokeSkiaPath(SKPath skiaPath, IShape shape)
        {
            IBrush? stroke = shape.Stroke;
            if (stroke != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Stroke, IsAntialias = true };
                InitSkiaPaintForBrush(paint, stroke, shape);
                paint.StrokeWidth = (int)shape.StrokeThickness;
                paint.StrokeMiter = (float)shape.StrokeMiterLimit;

                SKStrokeCap strokeCap = shape.StrokeLineCap switch
                {
                    PenLineCap.Flat => SKStrokeCap.Butt,
                    PenLineCap.Round => SKStrokeCap.Round,
                    PenLineCap.Square => SKStrokeCap.Square,
                    _ => throw new InvalidOperationException($"Unknown PenLineCap value {shape.StrokeLineCap}")
                };
                paint.StrokeCap = strokeCap;

                SKStrokeJoin strokeJoin = shape.StrokeLineJoin switch
                {
                    PenLineJoin.Miter => SKStrokeJoin.Miter,
                    PenLineJoin.Bevel => SKStrokeJoin.Bevel,
                    PenLineJoin.Round => SKStrokeJoin.Round,
                    _ => throw new InvalidOperationException($"Unknown PenLineJoin value {shape.StrokeLineJoin}")
                };
                paint.StrokeJoin = strokeJoin;

                skCanvas.DrawPath(skiaPath, paint);
            }
        }

        public static void InitSkiaPaintForBrush(SKPaint paint, IBrush brush, IShape shape)
        {
            if (brush is ISolidColorBrush solidColorBrush)
                paint.Color = ToSkiaColor(solidColorBrush.Color);
            else if (brush is IGradientBrush gradientBrush)
                paint.Shader = ToSkiaShader(gradientBrush, shape);
            else throw new InvalidOperationException($"Brush type {brush.GetType()} isn't currently supported");
        }

        public static SKColor ToSkiaColor(Color color) => new SKColor(color.R, color.G, color.B, color.A);

        public static SKShader ToSkiaShader(IGradientBrush gradientBrush, IShape shape)
        {
            SKShaderTileMode tileMode = gradientBrush.SpreadMethod switch
            {
                GradientSpreadMethod.Pad => SKShaderTileMode.Clamp,
                GradientSpreadMethod.Reflect => SKShaderTileMode.Mirror,
                GradientSpreadMethod.Repeat => SKShaderTileMode.Repeat,
                _ => throw new InvalidOperationException($"Unknown GradientSpreadMethod value {gradientBrush.SpreadMethod}")
            };

            List<SKColor> skiaColors = new List<SKColor>();
            List<float> skiaColorPositions = new List<float>();
            foreach (IGradientStop gradientStop in gradientBrush.GradientStops)
            {
                skiaColors.Add(ToSkiaColor(gradientStop.Color));
                skiaColorPositions.Add((float)gradientStop.Offset);
            }

            if (gradientBrush is ILinearGradientBrush linearGradientBrush)
            {
                SKPoint skiaStartPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.StartPoint, gradientBrush, shape);
                SKPoint skiaEndPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.EndPoint, gradientBrush, shape);

                return SKShader.CreateLinearGradient(skiaStartPoint, skiaEndPoint, skiaColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else if (gradientBrush is IRadialGradientBrush radialGradientBrush)
            {
                SKPoint skiaCenterPoint = GradientBrushPointToSkiaPoint(radialGradientBrush.Center, gradientBrush, shape);

                float radius = (float)(radialGradientBrush.RadiusX * shape.Width);
                return SKShader.CreateRadialGradient(skiaCenterPoint, radius, skiaColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else throw new InvalidOperationException($"GradientBrush type {gradientBrush.GetType()} is unknown");
        }

        public static SKPoint GradientBrushPointToSkiaPoint(Point point, IGradientBrush gradientBrush, IShape shape)
        {
            if (gradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                return new SKPoint(
                    (float)(shape.Left + point.X * shape.Width),
                    (float)(shape.Top + point.Y * shape.Height));
            else
                return new SKPoint((float)point.X, (float)point.Y);
        }

        public static SKPoint PointToSkiaPoint(Point point) => new SKPoint((float) point.X, (float) point.Y);

        public static void AddSkiaPoints(IEnumerable<Point> points, List<SKPoint> skiaPoints)
        {
            foreach (Point point in points)
                skiaPoints.Add(new SKPoint((float)point.X, (float)point.Y));
        }
    }
}
