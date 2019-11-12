using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;

namespace XGraphics.WPF.Converters
{
	public class PointsTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new Wrapper.Points(PointsConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
