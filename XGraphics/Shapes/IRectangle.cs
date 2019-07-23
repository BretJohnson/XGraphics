namespace XGraphics.Shapes
{
    public class IRectangleProperties
    {
        public static readonly XPlatBindableProperty RadiusXProperty = XPlatBindableProperty.Create(
            nameof(IRectangle.RadiusX), typeof(double), 0);

        public static readonly XPlatBindableProperty RadiusYProperty = XPlatBindableProperty.Create(
            nameof(IRectangle.RadiusY), typeof(double), 0);
    }

    public interface IRectangle : IShape
    {
        double RadiusX { get; set; }

        double RadiusY { get; set; }
    }
}
