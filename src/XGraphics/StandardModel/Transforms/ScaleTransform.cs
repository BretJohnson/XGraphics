// This file is generated from IScaleTransform.cs. Update the source file to change its contents.

using XGraphics.Transforms;

namespace XGraphics.StandardModel.Transforms
{
    public class ScaleTransform : Transform, IScaleTransform
    {
        public double CenterX { get; set; } = 0.0;

        public double CenterY { get; set; } = 0.0;

        public double ScaleX { get; set; } = 1.0;

        public double ScaleY { get; set; } = 1.0;
    }
}