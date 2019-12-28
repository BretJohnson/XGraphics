namespace XGraphics.XamarinForms.Wrapper
{
    //[TypeConverter(typeof(ImageSourceTypeConverter))]
    public abstract class ImageSource
    {
        public abstract XGraphics.ImageSource WrappedImageSource { get; }
    }
}
