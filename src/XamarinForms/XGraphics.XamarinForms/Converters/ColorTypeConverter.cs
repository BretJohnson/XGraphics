using XGraphics.Converters;

namespace XGraphics.XamarinForms.Converters
{
	public class ColorTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return new Wrapper.Color(ColorConverter.ConvertFromString(value));
        }
	}
}
