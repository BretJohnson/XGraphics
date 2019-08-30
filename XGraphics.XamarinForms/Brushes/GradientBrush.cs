using System.Collections.Generic;
using XGraphics.Brushes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Brushes
{
    public class GradientBrush : Brush, IGradientBrush
    {
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
    }
}