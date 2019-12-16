// This file is generated from IPolyBezierSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(PolyBezierSegment), Wrapper.Points.Default);

        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyBezierSegment.Points => Points.WrappedPoints;
    }
}