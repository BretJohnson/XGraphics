using System.ComponentModel;
using XGraphics.XamarinForms.Converters;
using XGraphicsColor = XGraphics.Color;

namespace XGraphics.XamarinForms.Wrapper
{
    [TypeConverter(typeof(ColorTypeConverter))]
    public struct Color
    {
        public static readonly Color Default = new Color(Colors.Default);
        public static readonly Color Transparent = new Color(Colors.Transparent);


        public XGraphicsColor WrappedColor { get; }

        public Color(XGraphicsColor wrappedColor)
        {
            WrappedColor = wrappedColor;
        }
    }
}
