// This file is generated from IBitmapImageSource.cs. Update the source file to change its contents.

using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class BitmapImageSource : LoadableImageSource, IBitmapImageSource
    {
        public static readonly BindableProperty DecodePixelWidthProperty = PropertyUtils.Create(nameof(DecodePixelWidth), typeof(int), typeof(BitmapImageSource), 0);
        public static readonly BindableProperty DecodePixelHeightProperty = PropertyUtils.Create(nameof(DecodePixelHeight), typeof(int), typeof(BitmapImageSource), 0);

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