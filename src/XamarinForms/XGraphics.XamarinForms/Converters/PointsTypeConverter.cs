using XGraphics.Converters;

namespace XGraphics.XamarinForms.Converters
{
	public class PointsTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return new Wrapper.Points(PointsConverter.ConvertFromString(value));
        }
	}
}
