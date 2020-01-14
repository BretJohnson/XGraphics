// This file is generated from IImage.cs. Update the source file to change its contents.

using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class Image : GraphicsElement, IImage
    {
        public static readonly BindableProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(ImageSource), typeof(Image), null);

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        IImageSource IImage.Source => Source;
    }
}