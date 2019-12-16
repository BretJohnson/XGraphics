// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Brushes;

namespace XGraphics.StandardModel.Brushes
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public GradientBrush()
        {
            GradientStops = new XGraphicsCollection<GradientStop>();
        }


        /// <summary>
        /// A collection of the GradientStop objects associated with the brush, each of which specifies a color and an offset along the brush's gradient axis. The default is an empty collection.
        /// </summary>
        public XGraphicsCollection<GradientStop> GradientStops { get; set; } = null;

        IEnumerable<IGradientStop> IGradientBrush.GradientStops => GradientStops;

        public BrushMappingMode MappingMode { get; set; } = BrushMappingMode.RelativeToBoundingBox;

        public GradientSpreadMethod SpreadMethod { get; set; } = GradientSpreadMethod.Pad;
    }
}