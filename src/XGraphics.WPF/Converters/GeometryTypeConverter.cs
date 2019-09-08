using System;
using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters.Path;
using XGraphics.Geometries;
using XGraphics.WPF.Geometries;

namespace XGraphics.WPF.Converters
{
	public class GeometryTypeConverter : TypeConverter
	{
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object valueObject)
        {
            if (!(valueObject is string value))
                throw new InvalidOperationException($"Cannot convert from type {valueObject.GetType()}");

            IPathGeometry pathGeometry = PathConverter.ParsePathGeometry(value, GeometryFactory.Instance);
            return pathGeometry;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
            Type destinationType)
        {
            return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            throw new InvalidOperationException($"ConvertTo isn't currently supported");
        }
	}
}