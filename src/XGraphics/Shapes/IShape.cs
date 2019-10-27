using XGraphics.Brushes;

namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IShape : IGraphicsElement
    {
        [ModelDefaultValue(double.NaN)]
        double Width { get; }

        [ModelDefaultValue(double.NaN)]
        double Height { get; }

        [ModelDefaultValue(null)]
        IBrush? Stroke { get; }

        [ModelDefaultValue(1.0)]
        double StrokeThickness { get; }

        [ModelDefaultValue(10.0)]
        double StrokeMiterLimit { get; }

        [ModelDefaultValue(PenLineCap.Flat)]
        PenLineCap StrokeLineCap { get; }

        [ModelDefaultValue(PenLineJoin.Miter)]
        PenLineJoin StrokeLineJoin { get; }

        [ModelDefaultValue(null)]
        IBrush? Fill { get; }
    }
}
