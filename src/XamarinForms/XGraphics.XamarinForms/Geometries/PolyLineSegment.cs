// This file is generated from IPolyLineSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PolyLineSegment : PathSegment, IPolyLineSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(PolyLineSegment), Wrapper.Points.Default);

        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyLineSegment.Points => Points.WrappedPoints;
    }
}