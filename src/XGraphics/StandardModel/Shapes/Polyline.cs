// This file is generated from IPolyline.cs. Update the source file to change its contents.

using XGraphics.Shapes;

namespace XGraphics.StandardModel.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public FillRule FillRule { get; set; } = FillRule.EvenOdd;

        public Points Points { get; set; } = Points.Default;
    }
}