// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Transforms;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly BindableProperty FiguresProperty = PropertyUtils.Create(nameof(Figures), typeof(XGraphicsCollection<PathFigure>), typeof(PathGeometry), null);
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(PathGeometry), FillRule.EvenOdd);

        public PathGeometry()
        {
            Figures = new XGraphicsCollection<PathFigure>();
        }

        public XGraphicsCollection<PathFigure> Figures
        {
            get => (XGraphicsCollection<PathFigure>)GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }
        IEnumerable<IPathFigure> IPathGeometry.Figures => Figures;

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}