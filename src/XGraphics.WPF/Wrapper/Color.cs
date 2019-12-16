using System.ComponentModel;
using XGraphics.WPF.Converters;

namespace XGraphics.WPF.Wrapper
{
    [TypeConverter(typeof(ColorTypeConverter))]
    public struct Color
    {
        public static readonly Color Default = new Color(XGraphics.Color.Default);
        public static readonly Color Transparent = new Color(Colors.Transparent);


        public XGraphics.Color WrappedColor { get; }

        public Color(XGraphics.Color wrappedColor)
        {
            WrappedColor = wrappedColor;
        }
    }
}
