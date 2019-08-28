using XGraphicsPoint = XGraphics.Point;

namespace XGraphics.XamarinForms.Wrapper
{
    public struct Point
    {
        public static readonly Point Default = new Point(XGraphicsPoint.Default);


        public XGraphicsPoint WrappedPoint { get; }

        public Point(XGraphicsPoint wrappedPoint)
        {
            WrappedPoint = wrappedPoint;
        }
    }
}
