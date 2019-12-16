// This file is generated from IShape.cs. Update the source file to change its contents.

using XGraphics.Brushes;
using XGraphics.StandardModel.Brushes;
using XGraphics.Shapes;

namespace XGraphics.StandardModel.Shapes
{
    public class Shape : GraphicsElement, IShape
    {
        public double Width { get; set; } = double.NaN;

        public double Height { get; set; } = double.NaN;


        /// <summary>
        /// A Brush that specifies how the Shape outline is painted. The default is null.
        /// </summary>
        public Brush? Stroke { get; set; } = null;

        IBrush? IShape.Stroke => Stroke;

        public double StrokeThickness { get; set; } = 1.0;

        public double StrokeMiterLimit { get; set; } = 10.0;

        public PenLineCap StrokeLineCap { get; set; } = PenLineCap.Flat;

        public PenLineJoin StrokeLineJoin { get; set; } = PenLineJoin.Miter;


        /// <summary>
        /// A Brush that paints/fills the shape interior. The default is null, (a null brush) which is evaluated as Transparent for rendering.
        /// </summary>
        public Brush? Fill { get; set; } = null;

        IBrush? IShape.Fill => Fill;
    }
}