using System.ComponentModel;
using XGraphics.WPF.Converters;

namespace XGraphics.WPF.Wrapper
{
    //[TypeConverter(typeof(ImageSourceTypeConverter))]
    public abstract class ImageSource
    {
        public abstract XGraphics.ImageSource WrappedImageSource { get; }
    }
}
