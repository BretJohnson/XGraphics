using XGraphics.Converters;

namespace XGraphics.XamarinForms.Converters
{
	public class PointTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return new Wrapper.Point(PointConverter.ConvertFromString(value));
        }
	}
}
