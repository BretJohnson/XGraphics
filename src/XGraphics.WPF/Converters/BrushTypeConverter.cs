using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;
using XGraphics.WPF.Brushes;

namespace XGraphics.WPF.Converters
{
	public class BrushTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new SolidColorBrush
            {
                Color = new Wrapper.Color(ColorConverter.ConvertFromString(GetValueAsString(valueObject)))
            };
        }
	}
}
