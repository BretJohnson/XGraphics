using System.ComponentModel;
using XGraphics.WPF.Converters;
using XGraphicsColor = XGraphics.Color;

namespace XGraphics.WPF.Wrapper
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
