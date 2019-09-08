using System.Collections.Generic;

namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IGradientBrush : IBrush
    {
        [ModelDefaultValue(GradientSpreadMethod.Pad)]
        GradientSpreadMethod SpreadMethod { get; }

        IEnumerable<IGradientStop> GradientStops { get; }
    }
}
