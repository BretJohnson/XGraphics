using XGraphics.Geometries;
using XGraphics.WPF.Geometries;
using XGraphics.Shapes;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly DependencyProperty DataProperty = PropertyUtils.Create(nameof(Data), typeof(Geometry), typeof(Path), null);

        IGeometry IPath.Data => Data;
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}