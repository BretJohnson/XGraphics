using XGraphics.Brushes;

namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IShape : IGraphicsElement
    {
        /// <summary>
        /// A Brush that specifies how the Shape outline is painted. The default is null.
        /// </summary>
        [ModelDefaultValue(null)]
        IBrush? Stroke { get; }

        /// <summary>
        /// The width of the Shape outline, in pixels. The default value is 0.
        /// </summary>
        [ModelDefaultValue(1.0)]
        double StrokeThickness { get; }

        /// <summary>
        /// The limit on the ratio of the miter length to the StrokeThickness of a Shape element. This value is always a positive number that is greater than or equal to 1.
        /// </summary>
        [ModelDefaultValue(10.0)]
        double StrokeMiterLimit { get; }

        /// <summary>
        /// A value of the PenLineCap enumeration that specifies the shape at the start of a Stroke. The default is Flat.
        /// </summary>
        [ModelDefaultValue(PenLineCap.Flat)]
        PenLineCap StrokeLineCap { get; }

        /// <summary>
        /// A value of the PenLineJoin enumeration that specifies the join appearance. The default value is Miter.
        /// </summary>
        [ModelDefaultValue(PenLineJoin.Miter)]
        PenLineJoin StrokeLineJoin { get; }

        /// <summary>
        /// A Brush that paints/fills the shape interior. The default is null, (a null brush) which is evaluated as Transparent for rendering.
        /// </summary>
        [ModelDefaultValue(null)]
        IBrush? Fill { get; }
    }
}
