using XGraphics.Transforms;

namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IGeometry
    {
        [ModelDefaultValue(0.25)]
        double StandardFlatteningTolerance { get; }

        [ModelDefaultValue(null)]
        ITransform Transform { get; }
    }
}
