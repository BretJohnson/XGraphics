using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;

namespace XGraphics.WPF.Converters
{
	public class ColorTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new Wrapper.Color(ColorConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
	}
}
