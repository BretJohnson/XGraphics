using System.ComponentModel;
using XGraphics.WPF.Converters;


namespace XGraphics.WPF.Wrapper
{
    [TypeConverter(typeof(PointsTypeConverter))]
    public struct Points
    {
        public static readonly Points Default = new Points(XGraphics.Points.Default);


        public XGraphics.Points WrappedPoints { get; }

        public Points(XGraphics.Points wrappedPoints)
        {
            WrappedPoints = wrappedPoints;
        }
    }
}
