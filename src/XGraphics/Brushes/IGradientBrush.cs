using System.Collections.Generic;

namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IGradientBrush : IBrush
    {
        /// <summary>
        /// A collection of the GradientStop objects associated with the brush, each of which specifies a color and an offset along the brush's gradient axis. The default is an empty collection.
        /// </summary>
        IEnumerable<IGradientStop> GradientStops { get; }

        /// <summary>
        /// A BrushMappingMode value that specifies how to interpret the gradient brush's positioning coordinates. The default is RelativeToBoundingBox.
        /// </summary>
        [ModelDefaultValue(BrushMappingMode.RelativeToBoundingBox)]
        BrushMappingMode MappingMode { get; }

        /// <summary>
        /// The type of spread method used to paint the gradient. The default is Pad.
        /// </summary>
        [ModelDefaultValue(GradientSpreadMethod.Pad)]
        GradientSpreadMethod SpreadMethod { get; }
    }
}
