using System;
using Xamarin.Forms;
using XGraphics.Converters.Path;
using XGraphics.Geometries;
using XGraphics.XamarinForms.Geometries;

namespace XGraphics.XamarinForms.Converters
{
	public class GeometryTypeConverter : TypeConverter
	{
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFromInvariantString(string value)
        {
            IPathGeometry pathGeometry = PathConverter.ParsePathGeometry(value, GeometryFactory.Instance);
            return pathGeometry;
        }
	}
}