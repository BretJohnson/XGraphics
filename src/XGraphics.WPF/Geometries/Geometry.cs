using XGraphics.Transforms;
using XGraphics.WPF.Transforms;
using XGraphics.Geometries;
using System.Windows;
using System.ComponentModel;
using System.Windows.Markup;
using XGraphics.WPF.Converters;

namespace XGraphics.WPF.Geometries
{
    [TypeConverter(typeof(GeometryTypeConverter))]
    public class Geometry : DependencyObjectWithCascadingNotifications, IGeometry
    {
        public static readonly DependencyProperty StandardFlatteningToleranceProperty = PropertyUtils.Create(nameof(StandardFlatteningTolerance), typeof(double), typeof(Geometry), 0.25);
        public static readonly DependencyProperty TransformProperty = PropertyUtils.Create(nameof(Transform), typeof(Transform), typeof(Geometry), null);

        public double StandardFlatteningTolerance
        {
            get => (double)GetValue(StandardFlatteningToleranceProperty);
            set => SetValue(StandardFlatteningToleranceProperty, value);
        }

        ITransform IGeometry.Transform => Transform;
        public Transform Transform
        {
            get => (Transform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }
    }
}