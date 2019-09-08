using System;
using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters;

namespace XGraphics.WPF.Converters
{
	public class PointTypeConverter : TypeConverter
	{
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            if (!(valueObject is string value))
                throw new InvalidOperationException($"Cannot convert from type {valueObject.GetType()}");

            return new Wrapper.Point(PointConverter.ConvertFromString(value));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => false;

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            throw new InvalidOperationException($"ConvertTo isn't currently supported");
    }
}