using System.ComponentModel;
using XGraphics.WPF.Converters;
using XGraphicsPoint = XGraphics.Point;


namespace XGraphics.WPF.Wrapper
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct Point
    {
        public static readonly Point Default = new Point(XGraphicsPoint.Default);
        public static readonly Point CenterDefault = new Point(XGraphicsPoint.CenterDefault);


        public XGraphicsPoint WrappedPoint { get; }

        public Point(XGraphicsPoint wrappedPoint)
        {
            WrappedPoint = wrappedPoint;
        }
    }
}
