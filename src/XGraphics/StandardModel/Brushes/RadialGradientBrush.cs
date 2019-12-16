// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.

using XGraphics.Brushes;

namespace XGraphics.StandardModel.Brushes
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public Point Center { get; set; } = Point.CenterDefault;

        public Point GradientOrigin { get; set; } = Point.CenterDefault;

        public double RadiusX { get; set; } = 0.5;
    }
}