// This file is generated from ITranslateTransform.cs. Update the source file to change its contents.
using XGraphics.Transforms;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Transforms
{
    public class TranslateTransform : Transform, ITranslateTransform
    {
        public static readonly DependencyProperty XProperty = PropertyUtils.Create(nameof(X), typeof(double), typeof(TranslateTransform), 0.0);
        public static readonly DependencyProperty YProperty = PropertyUtils.Create(nameof(Y), typeof(double), typeof(TranslateTransform), 0.0);

        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }
    }
}