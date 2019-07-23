namespace XGraphics.Shapes
{
    public class IShapeProperties
    {
        public static readonly XPlatBindableProperty LeftProperty = XPlatBindableProperty.Create(
            nameof(IShape.Left), typeof(double), double.NaN);

        public static readonly XPlatBindableProperty TopProperty = XPlatBindableProperty.Create(
            nameof(IShape.Top), typeof(double), double.NaN);

        public static readonly XPlatBindableProperty WidthProperty = XPlatBindableProperty.Create(
            nameof(IShape.Width), typeof(double), double.NaN);

        public static readonly XPlatBindableProperty HeightProperty = XPlatBindableProperty.Create(
            nameof(IShape.Height), typeof(double), double.NaN);

        public static readonly XPlatBindableProperty ZIndexProperty = XPlatBindableProperty.Create(
            nameof(IShape.ZIndex), typeof(int), 0);

        public static readonly XPlatBindableProperty StrokeProperty = XPlatBindableProperty.Create(
            nameof(IShape.Stroke), typeof(IBrush), null);

        public static readonly XPlatBindableProperty StrokeThicknessProperty = XPlatBindableProperty.Create(
            nameof(IShape.StrokeThickness), typeof(double), 0);

        public static readonly XPlatBindableProperty FillProperty = XPlatBindableProperty.Create(
            nameof(IShape.Fill), typeof(IBrush), null);
    }

    public interface IShape
    {
        double Left { get; }

        double Top { get; }

        double Width { get; }

        double Height { get; }

        int ZIndex { get; }

        IBrush? Stroke { get; }

        double StrokeThickness { get; }

        IBrush? Fill { get; }
    }
}
