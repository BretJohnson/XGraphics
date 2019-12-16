// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.

using XGraphics.Brushes;

namespace XGraphics.StandardModel.Brushes
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public Point StartPoint { get; set; } = Point.Default;

        public Point EndPoint { get; set; } = Point.Default;
    }
}