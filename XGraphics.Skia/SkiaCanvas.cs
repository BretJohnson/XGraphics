using System;
using SkiaSharp;
using XGraphics.Shapes;

namespace XGraphics.Skia
{
    public class SkiaCanvas : Canvas
    {
        private SKCanvas skCanvas;

        public SkiaCanvas(SKCanvas skCanvas)
        {
            this.skCanvas = skCanvas;
        }

        public override void DrawRectangle(Rectangle rectangle)
        {
            var rect = SKRect.Create((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);

            var fill = rectangle.Fill;
            if (fill != null)
            {
                using SKPaint paint = CreatePaintForFill(rectangle, fill);
                skCanvas.DrawRect(rect, paint);
            }

            var stroke = rectangle.Stroke;
            if (stroke != null && rectangle.StrokeThickness > 0.0)
            {
                using SKPaint paint = CreatePaintForStroke(rectangle, stroke);
                skCanvas.DrawRect(rect, paint);
            }
        }

        public static SKPaint CreatePaintForStroke(Shape shape, Brush stroke)
        {
            var paint = new SKPaint {Style = SKPaintStyle.Stroke, IsAntialias = true};

            if (stroke is SolidColorBrush solidColorBrush)
                paint.Color = ColorToSKColor(solidColorBrush.Color);
            else throw new InvalidOperationException($"Brush type {stroke.GetType()} isn't currently supported");
            paint.StrokeWidth = (int) shape.StrokeThickness;

            return paint;
        }

        public static SKPaint CreatePaintForFill(Shape shape, Brush fill)
        {
            var paint = new SKPaint {Style = SKPaintStyle.Fill, IsAntialias = true};

            if (fill is SolidColorBrush solidColorBrush)
                paint.Color = ColorToSKColor(solidColorBrush.Color);
            else throw new InvalidOperationException($"Brush type {fill.GetType()} isn't currently supported");

            return paint;
        }

        public static SKColor ColorToSKColor(Color color) =>
            new SKColor(color.R, color.G, color.B, color.A);
    }
}
