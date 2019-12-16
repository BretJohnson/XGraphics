// This file is generated from IPolyBezierSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(PolyBezierSegment), Wrapper.Points.Default);

        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyBezierSegment.Points => Points.WrappedPoints;
    }
}