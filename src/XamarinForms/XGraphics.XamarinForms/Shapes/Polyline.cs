// This file is generated from IPolyline.cs. Update the source file to change its contents.

using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(Polyline), FillRule.EvenOdd);
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(Polyline), Wrapper.Points.Default);

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }

        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyline.Points => Points.WrappedPoints;
    }
}