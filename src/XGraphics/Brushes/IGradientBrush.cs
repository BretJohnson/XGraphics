using System.Collections.Generic;

namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IGradientBrush : IBrush
    {
        IEnumerable<IGradientStop> GradientStops { get; }

        [ModelDefaultValue(BrushMappingMode.RelativeToBoundingBox)]
        BrushMappingMode MappingMode { get; }

        [ModelDefaultValue(GradientSpreadMethod.Pad)]
        GradientSpreadMethod SpreadMethod { get; }
    }
}
