using System.Collections.Generic;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(PathGeometry), FillRule.EvenOdd);

        public PathGeometry()
        {
            Figures = new GraphicsObjectCollection<PathFigure>();
            Figures.Changed += OnSubobjectChanged;
        }

        IEnumerable<IPathFigure> IPathGeometry.Figures => Figures;
        public GraphicsObjectCollection<PathFigure> Figures
        {
            get;
        }

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}