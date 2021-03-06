// This file is generated from IPath.cs. Update the source file to change its contents.

using XGraphics.Geometries;
using XGraphics.XamarinForms.Geometries;
using XGraphics.Shapes;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly BindableProperty DataProperty = PropertyUtils.Create(nameof(Data), typeof(Geometry), typeof(Path), null);

        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        IGeometry IPath.Data => Data;
    }
}