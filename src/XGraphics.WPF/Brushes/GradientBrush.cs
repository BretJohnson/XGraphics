// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Brushes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Brushes
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly DependencyProperty GradientStopsProperty = PropertyUtils.Create(nameof(GradientStops), typeof(XGraphicsCollection<GradientStop>), typeof(GradientBrush), null);
        public static readonly DependencyProperty MappingModeProperty = PropertyUtils.Create(nameof(MappingMode), typeof(BrushMappingMode), typeof(GradientBrush), BrushMappingMode.RelativeToBoundingBox);
        public static readonly DependencyProperty SpreadMethodProperty = PropertyUtils.Create(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(GradientBrush), GradientSpreadMethod.Pad);

        public GradientBrush()
        {
            GradientStops = new XGraphicsCollection<GradientStop>();
        }


        /// <summary>
        /// A collection of the GradientStop objects associated with the brush, each of which specifies a color and an offset along the brush's gradient axis. The default is an empty collection.
        /// </summary>
        public XGraphicsCollection<GradientStop> GradientStops
        {
            get => (XGraphicsCollection<GradientStop>)GetValue(GradientStopsProperty);
            set => SetValue(GradientStopsProperty, value);
        }
        IEnumerable<IGradientStop> IGradientBrush.GradientStops => GradientStops;

        public BrushMappingMode MappingMode
        {
            get => (BrushMappingMode)GetValue(MappingModeProperty);
            set => SetValue(MappingModeProperty, value);
        }

        public GradientSpreadMethod SpreadMethod
        {
            get => (GradientSpreadMethod)GetValue(SpreadMethodProperty);
            set => SetValue(SpreadMethodProperty, value);
        }
    }
}