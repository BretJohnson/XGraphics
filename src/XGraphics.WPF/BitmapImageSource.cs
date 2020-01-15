// This file is generated from IBitmapImageSource.cs. Update the source file to change its contents.

using XGraphics;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF
{
    public class BitmapImageSource : LoadableImageSource, IBitmapImageSource
    {
        public static readonly DependencyProperty DecodePixelWidthProperty = PropertyUtils.Create(nameof(DecodePixelWidth), typeof(int), typeof(BitmapImageSource), 0);
        public static readonly DependencyProperty DecodePixelHeightProperty = PropertyUtils.Create(nameof(DecodePixelHeight), typeof(int), typeof(BitmapImageSource), 0);

        public int DecodePixelWidth
        {
            get => (int)GetValue(DecodePixelWidthProperty);
            set => SetValue(DecodePixelWidthProperty, value);
        }

        public int DecodePixelHeight
        {
            get => (int)GetValue(DecodePixelHeightProperty);
            set => SetValue(DecodePixelHeightProperty, value);
        }
    }
}