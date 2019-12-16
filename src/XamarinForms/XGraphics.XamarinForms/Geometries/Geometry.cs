// This file is generated from IGeometry.cs. Update the source file to change its contents.

using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;
using XGraphics.Geometries;
using Xamarin.Forms;
using XGraphics.XamarinForms.Converters;

namespace XGraphics.XamarinForms.Geometries
{
    [TypeConverter(typeof(GeometryTypeConverter))]
    public class Geometry : BindableObjectWithCascadingNotifications, IGeometry
    {
        public static readonly BindableProperty StandardFlatteningToleranceProperty = PropertyUtils.Create(nameof(StandardFlatteningTolerance), typeof(double), typeof(Geometry), 0.25);
        public static readonly BindableProperty TransformProperty = PropertyUtils.Create(nameof(Transform), typeof(Transform), typeof(Geometry), null);

        public double StandardFlatteningTolerance
        {
            get => (double)GetValue(StandardFlatteningToleranceProperty);
            set => SetValue(StandardFlatteningToleranceProperty, value);
        }

        public Transform Transform
        {
            get => (Transform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }
        ITransform IGeometry.Transform => Transform;
    }
}