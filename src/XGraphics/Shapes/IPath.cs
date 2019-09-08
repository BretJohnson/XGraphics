using XGraphics.Geometries;

namespace XGraphics.Shapes
{
    [GraphicsModelObject]
    public interface IPath : IShape
    {
        [ModelDefaultValue(null)]
        IGeometry Data { get; }
    }
}
