using Xamarin.Forms;
using PointTypeConverter = XGraphics.XamarinForms.Converters.PointTypeConverter;

namespace XGraphics.XamarinForms.Wrapper
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct Point
    {
        public static readonly Point Default = new Point(XGraphics.Point.Default);
        public static readonly Point CenterDefault = new Point(XGraphics.Point.CenterDefault);


        public XGraphics.Point WrappedPoint { get; }

        public Point(XGraphics.Point wrappedPoint)
        {
            WrappedPoint = wrappedPoint;
        }
    }
}
