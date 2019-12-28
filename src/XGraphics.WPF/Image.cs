// This file is generated from IImage.cs. Update the source file to change its contents.

using XGraphics;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF
{
    public class Image : GraphicsElement, IImage
    {
        public static readonly DependencyProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(Wrapper.ImageSource), typeof(Image), null);

        public Wrapper.ImageSource Source
        {
            get => (Wrapper.ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        ImageSource IImage.Source => Source.WrappedImageSource;
    }
}
