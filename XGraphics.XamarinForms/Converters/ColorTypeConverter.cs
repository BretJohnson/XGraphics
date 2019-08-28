using System;
using Xamarin.Forms;
using XGraphics.Converters;

namespace XGraphics.XamarinForms.Converters
{
	public class ColorTypeConverter : TypeConverter
	{
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFromInvariantString(string value)
        {
            return new Wrapper.Color(ColorConverter.ConvertFromString(value));
        }
	}
}