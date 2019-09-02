using System;
using System.Globalization;

namespace XGraphics.Converters
{
	public static class PointConverter
	{
        public static Point ConvertFromString(string value)
        {

            if (value != null)
            {
                double x, y;
                string[] xy = value.Split(',');
                if (xy.Length == 2 && double.TryParse(xy[0], NumberStyles.Number, CultureInfo.InvariantCulture, out x) && double.TryParse(xy[1], NumberStyles.Number, CultureInfo.InvariantCulture, out y))
                    return new Point(x, y);
            }

            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(Point)));
        }
	}
}