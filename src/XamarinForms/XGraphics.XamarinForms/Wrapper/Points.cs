using Xamarin.Forms;
using XGraphics.XamarinForms.Converters;

namespace XGraphics.XamarinForms.Wrapper
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
