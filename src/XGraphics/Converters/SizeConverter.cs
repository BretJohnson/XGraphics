using System;
using System.Globalization;

namespace XGraphics.Converters
{
	public static class SizeConverter
	{
        public static Size ConvertFromString(string value)
        {
            if (value != null)
            {
                string[] wh = value.Split(',');
                if (wh.Length == 2
                    && double.TryParse(wh[0], NumberStyles.Number, CultureInfo.InvariantCulture, out double w)
                    && double.TryParse(wh[1], NumberStyles.Number, CultureInfo.InvariantCulture, out double h))
                    return new Size(w, h);
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
        }
	}
}
