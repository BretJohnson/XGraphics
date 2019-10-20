using System.ComponentModel;
using XGraphics.WPF.Converters;


namespace XGraphics.WPF.Wrapper
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
