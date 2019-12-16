// This file is generated from ILine.cs. Update the source file to change its contents.

using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Line : Shape, ILine
    {
        public static readonly BindableProperty X1Property = PropertyUtils.Create(nameof(X1), typeof(double), typeof(Line), 0.0);
        public static readonly BindableProperty Y1Property = PropertyUtils.Create(nameof(Y1), typeof(double), typeof(Line), 0.0);
        public static readonly BindableProperty X2Property = PropertyUtils.Create(nameof(X2), typeof(double), typeof(Line), 0.0);
        public static readonly BindableProperty Y2Property = PropertyUtils.Create(nameof(Y2), typeof(double), typeof(Line), 0.0);

        public double X1
        {
            get => (double)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }
    }
}