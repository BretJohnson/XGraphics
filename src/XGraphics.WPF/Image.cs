// This file is generated from IImage.cs. Update the source file to change its contents.

using XGraphics;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF
{
    public class Image : GraphicsElement, IImage
    {
        public static readonly DependencyProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(ImageSource), typeof(Image), null);

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        IImageSource IImage.Source => Source;
    }
}