using System;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Converters
{
	public class TypeConverterBase : TypeConverter
	{
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }
	}
}
