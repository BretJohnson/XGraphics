using Xamarin.Forms;
using ColorTypeConverter = XGraphics.XamarinForms.Converters.ColorTypeConverter;

namespace XGraphics.XamarinForms.Wrapper
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
