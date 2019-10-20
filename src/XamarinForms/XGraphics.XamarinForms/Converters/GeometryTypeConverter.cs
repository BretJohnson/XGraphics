using XGraphics.Converters.Path;
using XGraphics.XamarinForms.Geometries;

namespace XGraphics.XamarinForms.Converters
{
	public class GeometryTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return PathConverter.ParsePathGeometry(value, GeometryFactory.Instance);
        }
	}
}
