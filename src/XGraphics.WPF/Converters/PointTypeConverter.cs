using System;
using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;

namespace XGraphics.WPF.Converters
{
	public class PointTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new Wrapper.Point(PointConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
