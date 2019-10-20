// This file is generated from IGradientBrush.cs. Update the source file to change its contents.
using System.Collections.Generic;
using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly BindableProperty MappingModeProperty = PropertyUtils.Create(nameof(MappingMode), typeof(BrushMappingMode), typeof(GradientBrush), BrushMappingMode.RelativeToBoundingBox);
        public static readonly BindableProperty SpreadMethodProperty = PropertyUtils.Create(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(GradientBrush), GradientSpreadMethod.Pad);

        public GradientBrush()
        {
            GradientStops = new GraphicsObjectCollection<GradientStop>();
            GradientStops.Changed += OnSubobjectChanged;
        }

        IEnumerable<IGradientStop> IGradientBrush.GradientStops => GradientStops;
        public GraphicsObjectCollection<GradientStop> GradientStops
        {
            get;
        }

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