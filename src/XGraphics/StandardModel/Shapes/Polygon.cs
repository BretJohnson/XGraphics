// This file is generated from IPolygon.cs. Update the source file to change its contents.

using XGraphics.Shapes;

namespace XGraphics.StandardModel.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public FillRule FillRule { get; set; } = FillRule.EvenOdd;

        public Points Points { get; set; } = Points.Default;
    }
}