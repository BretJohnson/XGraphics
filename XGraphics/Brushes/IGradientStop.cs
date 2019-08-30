namespace XGraphics.Brushes
{
    [GraphicsModelObject]
    public interface IGradientStop
    {
        // The default is Transparent
        Color Color { get; }

        [ModelDefaultValue(0.0)]
        double Offset { get; }
    }
}
