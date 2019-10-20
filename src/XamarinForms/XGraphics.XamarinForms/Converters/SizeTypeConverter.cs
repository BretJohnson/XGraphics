using XGraphics.Converters;

namespace XGraphics.XamarinForms.Converters
{
	public class SizeTypeConverter : TypeConverterBase
	{
        public override object ConvertFromInvariantString(string value)
        {
            return new Wrapper.Size(SizeConverter.ConvertFromString(value));
        }
	}
}
