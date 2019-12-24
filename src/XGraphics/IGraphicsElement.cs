using XGraphics.Shapes;
using XGraphics.Transforms;

namespace XGraphics
{
    [GraphicsModelObject]
    public interface IGraphicsElement
    {
        [ModelDefaultValue(0.0)]
        double Left { get; }

        [ModelDefaultValue(0.0)]
        double Top { get; }

        [ModelDefaultValue(double.NaN)]
        double Width { get; }

        [ModelDefaultValue(double.NaN)]
        double Height { get; }

        [ModelDefaultValue(null)]
        ITransform? RenderTransform { get; }
    }
}
