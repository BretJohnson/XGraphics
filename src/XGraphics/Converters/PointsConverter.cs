using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XGraphics.Converters
{
	public static class PointsConverter
	{
        public static Points ConvertFromString(string value)
        {
            return Parse(value);
        }

        /// <summary>
        /// Parse - returns an instance converted from the provided string
        /// using the current culture
        /// <param name="source"> string with PointCollection data </param>
        /// </summary>
        public static Points Parse(string source)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);
            List<Point> pointsList = new List<Point>();

            while (th.NextToken())
            {
                Point point = new Point(
                    Convert.ToDouble(th.GetCurrentToken(), formatProvider),
                    Convert.ToDouble(th.NextTokenRequired(), formatProvider));
                pointsList.Add(point);
            }

            return new Points(pointsList.ToArray());
        }

        public static string ConvertToString(Points points)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;

            StringBuilder buffer = new StringBuilder();
            int length = points.Length;
            for (int i = 0; i < length; i++)
            {
                if (buffer.Length > 0)
                    buffer.Append(" ");

                Point point = points[i];
                buffer.Append(point.X.ToString(formatProvider));
                buffer.Append(",");
                buffer.Append(point.Y.ToString(formatProvider));
            }

            return buffer.ToString();
        }
    }
}
