using System.Collections.Generic;

namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IGradientBrush : IBrush
    {
        IEnumerable<IGradientStop> GradientStops { get; }
    }
}
