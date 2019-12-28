// This file is generated from IImage.cs. Update the source file to change its contents.

using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class Image : GraphicsElement, IImage
    {
        public static readonly BindableProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(Wrapper.ImageSource), typeof(Image), null);

        public Wrapper.ImageSource Source
        {
            get => (Wrapper.ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        ImageSource IImage.Source => Source.WrappedImageSource;
    }
}