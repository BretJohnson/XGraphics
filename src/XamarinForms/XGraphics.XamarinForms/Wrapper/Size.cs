using Xamarin.Forms;
using SizeTypeConverter = XGraphics.XamarinForms.Converters.SizeTypeConverter;

namespace XGraphics.XamarinForms.Wrapper
{
    [TypeConverter(typeof(SizeTypeConverter))]
    public struct Size
    {
        public static readonly Size Default = new Size(XGraphics.Size.Default);


        public XGraphics.Size WrappedSize { get; }

        public Size(XGraphics.Size wrappedSize)
        {
            WrappedSize = wrappedSize;
        }
    }
}
