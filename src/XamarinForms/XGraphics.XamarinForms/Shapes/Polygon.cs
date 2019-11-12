// This file is generated from IPolygon.cs. Update the source file to change its contents.
using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(Polygon), FillRule.EvenOdd);
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(Polygon), Wrapper.Points.Default);

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }

        Points IPolygon.Points => Points.WrappedPoints;
        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}