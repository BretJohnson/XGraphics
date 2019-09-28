// This file is generated from IPathGeometry.cs. Update the source file to change its contents.
using System.Collections.Generic;
using XGraphics.Transforms;
using XGraphics.WPF.Transforms;
using XGraphics.Geometries;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Geometries
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly DependencyProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(PathGeometry), FillRule.EvenOdd);

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