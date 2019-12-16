// This file is generated from IRotateTransform.cs. Update the source file to change its contents.

using XGraphics.Transforms;

namespace XGraphics.StandardModel.Transforms
{
    public class RotateTransform : Transform, IRotateTransform
    {
        public double Angle { get; set; } = 0.0;

        public double CenterX { get; set; } = 0.0;

        public double CenterY { get; set; } = 0.0;
    }
}