namespace XGraphics
{
    public class SolidColorBrushProperties
    {
        public static readonly XPlatBindableProperty ColorProperty = XPlatBindableProperty.Create(
            nameof(ISolidColorBrush.Color), typeof(Color), new Color());
    }

    public interface ISolidColorBrush : IBrush
    {
        Color Color { get; }
    }
}
