using System;
using System.Collections.Generic;
using System.Globalization;

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
    }
}
