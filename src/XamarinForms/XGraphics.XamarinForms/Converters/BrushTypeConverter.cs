using XGraphics.Converters;
using XGraphics.XamarinForms.Brushes;

namespace XGraphics.XamarinForms.Converters
{
	public class BrushTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return new SolidColorBrush
            {
                Color = new Wrapper.Color(ColorConverter.ConvertFromString(value))
            };
        }
	}
}
