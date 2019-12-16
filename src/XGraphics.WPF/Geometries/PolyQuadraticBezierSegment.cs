// This file is generated from IPolyQuadraticBezierSegment.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PolyQuadraticBezierSegment : PathSegment, IPolyQuadraticBezierSegment
    {
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(Wrapper.Points), typeof(PolyQuadraticBezierSegment), Wrapper.Points.Default);

        public Wrapper.Points Points
        {
            get => (Wrapper.Points)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyQuadraticBezierSegment.Points => Points.WrappedPoints;
    }
}