namespace XGraphics.Shapes
{
    public class Shape
    {
        public double Left { get; set; } = double.NaN;

        public double Top { get; set; } = double.NaN;

        public double Width { get; set; } = double.NaN;

        public double Height { get; set; } = double.NaN;

        public int ZIndex { get; set; } = 0;

        public Brush? Stroke { get; set; } = null;

        public double StrokeThickness { get; set; } = 0.0;

        public Brush? Fill { get; set; } = null;
    }
}
