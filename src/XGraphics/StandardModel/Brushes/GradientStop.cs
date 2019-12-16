// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using XGraphics.Brushes;

namespace XGraphics.StandardModel.Brushes
{
    public class GradientStop : ObjectWithCascadingNotifications, IGradientStop
    {
        public Color Color { get; set; } = Color.Default;

        public double Offset { get; set; } = 0.0;
    }
}