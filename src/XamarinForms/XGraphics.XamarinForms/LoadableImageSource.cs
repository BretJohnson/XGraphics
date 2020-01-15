// This file is generated from ILoadableImageSource.cs. Update the source file to change its contents.

using System;
using XGraphics.ImageLoading;
using XGraphics.XamarinForms.ImageLoading;
using XGraphics;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class LoadableImageSource : ImageSource, ILoadableImageSource
    {
        public static readonly BindableProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(Wrapper.DataSource), typeof(LoadableImageSource), null);
        public static readonly BindableProperty LoadingStatusProperty = PropertyUtils.Create(nameof(LoadingStatus), typeof(LoadingStatus), typeof(LoadableImageSource), LoadingStatus.NotStarted);
        public static readonly BindableProperty LoadedImageProperty = PropertyUtils.Create(nameof(LoadedImage), typeof(LoadedImage), typeof(LoadableImageSource), null);
        public static readonly BindableProperty LoadingErrorProperty = PropertyUtils.Create(nameof(LoadingError), typeof(Exception), typeof(LoadableImageSource), null);
        public static readonly BindableProperty DecoderProperty = PropertyUtils.Create(nameof(Decoder), typeof(ImageDecoder), typeof(LoadableImageSource), null);

        public Wrapper.DataSource Source
        {
            get => (Wrapper.DataSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        DataSource ILoadableImageSource.Source => Source.WrappedDataSource;

        public LoadingStatus LoadingStatus
        {
            get => (LoadingStatus)GetValue(LoadingStatusProperty);
            set => SetValue(LoadingStatusProperty, value);
        }

        public LoadedImage? LoadedImage
        {
            get => (LoadedImage?)GetValue(LoadedImageProperty);
            set => SetValue(LoadedImageProperty, value);
        }

        public Exception? LoadingError
        {
            get => (Exception?)GetValue(LoadingErrorProperty);
            set => SetValue(LoadingErrorProperty, value);
        }

        public ImageDecoder Decoder
        {
            get => (ImageDecoder)GetValue(DecoderProperty);
            set => SetValue(DecoderProperty, value);
        }
    }
}