// This file is generated from ILoadableImageSource.cs. Update the source file to change its contents.

using System;
using XGraphics.ImageLoading;
using XGraphics;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF
{
    public class LoadableImageSource : ImageSource, ILoadableImageSource
    {
        public static readonly DependencyProperty SourceProperty = PropertyUtils.Create(nameof(Source), typeof(Wrapper.DataSource), typeof(LoadableImageSource), null);
        public static readonly DependencyProperty LoadingStatusProperty = PropertyUtils.Create(nameof(LoadingStatus), typeof(LoadingStatus), typeof(LoadableImageSource), LoadingStatus.NotStarted);
        public static readonly DependencyProperty LoadedImageProperty = PropertyUtils.Create(nameof(LoadedImage), typeof(LoadedImage), typeof(LoadableImageSource), null);
        public static readonly DependencyProperty LoadingErrorProperty = PropertyUtils.Create(nameof(LoadingError), typeof(Exception), typeof(LoadableImageSource), null);
        public static readonly DependencyProperty DecoderProperty = PropertyUtils.Create(nameof(Decoder), typeof(ImageDecoder), typeof(LoadableImageSource), null);

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
