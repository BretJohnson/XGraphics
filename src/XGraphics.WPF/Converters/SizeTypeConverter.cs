using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;

namespace XGraphics.WPF.Converters
{
	public class SizeTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new Wrapper.Size(SizeConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
