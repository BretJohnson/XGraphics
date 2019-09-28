// This file is generated from IScaleTransform.cs. Update the source file to change its contents.
using XGraphics.Transforms;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Transforms
{
    public class ScaleTransform : Transform, IScaleTransform
    {
        public static readonly DependencyProperty CenterXProperty = PropertyUtils.Create(nameof(CenterX), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly DependencyProperty CenterYProperty = PropertyUtils.Create(nameof(CenterY), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly DependencyProperty ScaleXProperty = PropertyUtils.Create(nameof(ScaleX), typeof(double), typeof(ScaleTransform), 1.0);
        public static readonly DependencyProperty ScaleYProperty = PropertyUtils.Create(nameof(ScaleY), typeof(double), typeof(ScaleTransform), 1.0);

        public double CenterX
        {
            get => (double)GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }

        public double CenterY
        {
            get => (double)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        public double ScaleX
        {
            get => (double)GetValue(ScaleXProperty);
            set => SetValue(ScaleXProperty, value);
        }

        public double ScaleY
        {
            get => (double)GetValue(ScaleYProperty);
            set => SetValue(ScaleYProperty, value);
        }
    }
}