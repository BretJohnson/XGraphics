using System.ComponentModel;
using System.Globalization;
using XGraphics.Converters.Path;
using XGraphics.WPF.Geometries;

namespace XGraphics.WPF.Converters
{
	public class GeometryTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return PathConverter.ParsePathGeometry(GetValueAsString(valueObject), GeometryFactory.Instance);
        }
	}
}
