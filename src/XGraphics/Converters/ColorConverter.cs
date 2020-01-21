using System;
using System.Collections.Generic;

namespace XGraphics.Converters
{
    public static class ColorConverter
    {
        private static Lazy<Dictionary<string, Color>> NamesToColors = new Lazy<Dictionary<string, Color>>(CreateNamesToColors);
        private static Lazy<Dictionary<Color, string>> ColorsToNames = new Lazy<Dictionary<Color, string>>(CreateColorsToNames);

        public static Color ConvertFromString(string value)
        {
            return Parse(value);
        }

        // Supported inputs
        // HEX		#rgb, #argb, #rrggbb, #aarrggbb
        // RGB		rgb(255,0,0), rgb(100%,0%,0%)					values in range 0-255 or 0%-100%
        // RGBA		rgba(255, 0, 0, 0.8), rgba(100%, 0%, 0%, 0.8)	opacity is 0.0-1.0
        // HSL		hsl(120, 100%, 50%)								h is 0-360, s and l are 0%-100%
        // HSLA		hsla(120, 100%, 50%, .8)						opacity is 0.0-1.0
        // Predefined color											case insensitive
        public static Color Parse(string value)
        {
            if (value == null)
                throw new InvalidOperationException($"Cannot convert null into a Color");

            value = value.Trim();
            if (value.StartsWith("#", StringComparison.Ordinal) && Color.FromHex(value, out Color hexColor))
                return hexColor;
#if LATER

            if (value.StartsWith("rgba", StringComparison.OrdinalIgnoreCase)) {
				var op = value.IndexOf('(');
				var cp = value.LastIndexOf(')');
				if (op < 0 || cp < 0 || cp < op)
					throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
				var quad = value.Substring(op + 1, cp - op - 1).Split(',');
				if (quad.Length != 4)
					throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
				var r = ParseColorValue(quad[0], 255, acceptPercent: true);
				var g = ParseColorValue(quad[1], 255, acceptPercent: true);
				var b = ParseColorValue(quad[2], 255, acceptPercent: true);
				var a = ParseOpacity(quad[3]);
				return Color.FromArgb(a, r, g, b);
			}

			if (value.StartsWith("rgb", StringComparison.OrdinalIgnoreCase)) {
				var op = value.IndexOf('(');
				var cp = value.LastIndexOf(')');
				if (op < 0 || cp < 0 || cp < op)
					throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
				var triplet = value.Substring(op + 1, cp - op - 1).Split(',');
				if (triplet.Length != 3)
					throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
				var r = ParseColorValue(triplet[0], 255, acceptPercent: true);
				var g = ParseColorValue(triplet[1], 255, acceptPercent: true);
				var b = ParseColorValue(triplet[2], 255, acceptPercent: true);
				return Color.FromRgb(r, g, b);
			}

            if (value.StartsWith("hsla", StringComparison.OrdinalIgnoreCase))
            {
                var op = value.IndexOf('(');
                var cp = value.LastIndexOf(')');
                if (op < 0 || cp < 0 || cp < op)
                    throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
                var quad = value.Substring(op + 1, cp - op - 1).Split(',');
                if (quad.Length != 4)
                    throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
                var h = ParseColorValue(quad[0], 360, acceptPercent: false);
                var s = ParseColorValue(quad[1], 100, acceptPercent: true);
                var l = ParseColorValue(quad[2], 100, acceptPercent: true);
                var a = ParseOpacity(quad[3]);
                return Color.FromHsla(h, s, l, a);
            }

            if (value.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
            {
                var op = value.IndexOf('(');
                var cp = value.LastIndexOf(')');
                if (op < 0 || cp < 0 || cp < op)
                    throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
                var triplet = value.Substring(op + 1, cp - op - 1).Split(',');
                if (triplet.Length != 3)
                    throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Color)}");
                var h = ParseColorValue(triplet[0], 360, acceptPercent: false);
                var s = ParseColorValue(triplet[1], 100, acceptPercent: true);
                var l = ParseColorValue(triplet[2], 100, acceptPercent: true);
                return Color.FromHsla(h, s, l);
            }
#endif

            string[] parts = value.Split('.');
            if (parts.Length == 1 || (parts.Length == 2 && parts[0] == "Color"))
            {
                string colorName = parts[parts.Length - 1].ToLowerInvariant();
                if (NamesToColors.Value.TryGetValue(colorName, out Color color))
                    return color;
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into a Color");
        }

        private static Dictionary<string, Color> CreateNamesToColors()
        {
            return new Dictionary<string, Color>
            {
                ["aliceblue"] = Colors.AliceBlue,
                ["antiquewhite"] = Colors.AntiqueWhite,
                ["aqua"] = Colors.Aqua,
                ["aquamarine"] = Colors.Aquamarine,
                ["azure"] = Colors.Azure,
                ["beige"] = Colors.Beige,
                ["bisque"] = Colors.Bisque,
                ["black"] = Colors.Black,
                ["blanchedalmond"] = Colors.BlanchedAlmond,
                ["blue"] = Colors.Blue,
                ["blueViolet"] = Colors.BlueViolet,
                ["brown"] = Colors.Brown,
                ["burlywood"] = Colors.BurlyWood,
                ["cadetblue"] = Colors.CadetBlue,
                ["chartreuse"] = Colors.Chartreuse,
                ["chocolate"] = Colors.Chocolate,
                ["coral"] = Colors.Coral,
                ["cornflowerblue"] = Colors.CornflowerBlue,
                ["cornsilk"] = Colors.Cornsilk,
                ["crimson"] = Colors.Crimson,
                ["cyan"] = Colors.Cyan,
                ["darkblue"] = Colors.DarkBlue,
                ["darkcyan"] = Colors.DarkCyan,
                ["darkgoldenrod"] = Colors.DarkGoldenrod,
                ["darkgray"] = Colors.DarkGray,
                ["darkgreen"] = Colors.DarkGreen,
                ["darkkhaki"] = Colors.DarkKhaki,
                ["darkmagenta"] = Colors.DarkMagenta,
                ["darkolivegreen"] = Colors.DarkOliveGreen,
                ["darkorange"] = Colors.DarkOrange,
                ["darkorchid"] = Colors.DarkOrchid,
                ["darkred"] = Colors.DarkRed,
                ["darksalmon"] = Colors.DarkSalmon,
                ["darkseagreen"] = Colors.DarkSeaGreen,
                ["darkslateblue"] = Colors.DarkSlateBlue,
                ["darkslategray"] = Colors.DarkSlateGray,
                ["darkturquoise"] = Colors.DarkTurquoise,
                ["darkviolet"] = Colors.DarkViolet,
                ["deeppink"] = Colors.DeepPink,
                ["deepskyblue"] = Colors.DeepSkyBlue,
                ["dimgray"] = Colors.DimGray,
                ["dodgerblue"] = Colors.DodgerBlue,
                ["firebrick"] = Colors.Firebrick,
                ["floralwhite"] = Colors.FloralWhite,
                ["forestgreen"] = Colors.ForestGreen,
                ["fuchsia"] = Colors.Fuchsia,
                ["gainsboro"] = Colors.Gainsboro,
                ["ghostwhite"] = Colors.GhostWhite,
                ["gold"] = Colors.Gold,
                ["goldenrod"] = Colors.Goldenrod,
                ["gray"] = Colors.Gray,
                ["green"] = Colors.Green,
                ["greenyellow"] = Colors.GreenYellow,
                ["honeydew"] = Colors.Honeydew,
                ["hotpink"] = Colors.HotPink,
                ["indianred"] = Colors.IndianRed,
                ["indigo"] = Colors.Indigo,
                ["ivory"] = Colors.Ivory,
                ["khaki"] = Colors.Khaki,
                ["lavender"] = Colors.Lavender,
                ["lavenderblush"] = Colors.LavenderBlush,
                ["lawngreen"] = Colors.LawnGreen,
                ["lemonchiffon"] = Colors.LemonChiffon,
                ["lightblue"] = Colors.LightBlue,
                ["lightcoral"] = Colors.LightCoral,
                ["lightcyan"] = Colors.LightCyan,
                ["lightgoldenrodyellow"] = Colors.LightGoldenrodYellow,
                ["lightgrey"] = Colors.LightGray,
                ["lightgray"] = Colors.LightGray,
                ["lightgreen"] = Colors.LightGreen,
                ["lightpink"] = Colors.LightPink,
                ["lightsalmon"] = Colors.LightSalmon,
                ["lightseagreen"] = Colors.LightSeaGreen,
                ["lightskyblue"] = Colors.LightSkyBlue,
                ["lightslategray"] = Colors.LightSlateGray,
                ["lightsteelblue"] = Colors.LightSteelBlue,
                ["lightyellow"] = Colors.LightYellow,
                ["lime"] = Colors.Lime,
                ["limegreen"] = Colors.LimeGreen,
                ["linen"] = Colors.Linen,
                ["magenta"] = Colors.Magenta,
                ["maroon"] = Colors.Maroon,
                ["mediumaquamarine"] = Colors.MediumAquamarine,
                ["mediumblue"] = Colors.MediumBlue,
                ["mediumorchid"] = Colors.MediumOrchid,
                ["mediumpurple"] = Colors.MediumPurple,
                ["mediumseagreen"] = Colors.MediumSeaGreen,
                ["mediumslateblue"] = Colors.MediumSlateBlue,
                ["mediumspringgreen"] = Colors.MediumSpringGreen,
                ["mediumturquoise"] = Colors.MediumTurquoise,
                ["mediumvioletred"] = Colors.MediumVioletRed,
                ["midnightblue"] = Colors.MidnightBlue,
                ["mintcream"] = Colors.MintCream,
                ["mistyrose"] = Colors.MistyRose,
                ["moccasin"] = Colors.Moccasin,
                ["navajowhite"] = Colors.NavajoWhite,
                ["navy"] = Colors.Navy,
                ["oldlace"] = Colors.OldLace,
                ["olive"] = Colors.Olive,
                ["olivedrab"] = Colors.OliveDrab,
                ["orange"] = Colors.Orange,
                ["orangered"] = Colors.OrangeRed,
                ["orchid"] = Colors.Orchid,
                ["palegoldenrod"] = Colors.PaleGoldenrod,
                ["palegreen"] = Colors.PaleGreen,
                ["paleturquoise"] = Colors.PaleTurquoise,
                ["palevioletred"] = Colors.PaleVioletRed,
                ["papayawhip"] = Colors.PapayaWhip,
                ["peachpuff"] = Colors.PeachPuff,
                ["peru"] = Colors.Peru,
                ["pink"] = Colors.Pink,
                ["plum"] = Colors.Plum,
                ["powderblue"] = Colors.PowderBlue,
                ["purple"] = Colors.Purple,
                ["red"] = Colors.Red,
                ["rosybrown"] = Colors.RosyBrown,
                ["royalblue"] = Colors.RoyalBlue,
                ["saddlebrown"] = Colors.SaddleBrown,
                ["salmon"] = Colors.Salmon,
                ["sandybrown"] = Colors.SandyBrown,
                ["seagreen"] = Colors.SeaGreen,
                ["seashell"] = Colors.SeaShell,
                ["sienna"] = Colors.Sienna,
                ["silver"] = Colors.Silver,
                ["skyblue"] = Colors.SkyBlue,
                ["slateblue"] = Colors.SlateBlue,
                ["slategray"] = Colors.SlateGray,
                ["snow"] = Colors.Snow,
                ["springgreen"] = Colors.SpringGreen,
                ["steelblue"] = Colors.SteelBlue,
                ["tan"] = Colors.Tan,
                ["teal"] = Colors.Teal,
                ["thistle"] = Colors.Thistle,
                ["tomato"] = Colors.Tomato,
                ["transparent"] = Colors.Transparent,
                ["turquoise"] = Colors.Turquoise,
                ["violet"] = Colors.Violet,
                ["wheat"] = Colors.Wheat,
                ["white"] = Colors.White,
                ["whitesmoke"] = Colors.WhiteSmoke,
                ["yellow"] = Colors.Yellow,
                ["yellowgreen"] = Colors.YellowGreen
            };
        }

        private static Dictionary<Color, string> CreateColorsToNames()
        {
            var colorsToNames = new Dictionary<Color, string>();
            foreach (KeyValuePair<string, Color> pair in NamesToColors.Value)
            {
                string name = pair.Key;

                // A few colors have name aliases. Prefer "cyan" to "aqua", "magenta" to "fuchsia", and "gray" to "grey"
                if (name == "aqua" || name == "fuchsia" || name.EndsWith("grey"))
                    continue;

                colorsToNames.Add(pair.Value, pair.Key);
            }

            return colorsToNames;
        }

        public static string ConvertToString(Color color)
        {
            if (ColorsToNames.Value.TryGetValue(color, out string colorName))
                return colorName;

            if (color.A == 0xFF)
                return $"#{ToHex(color.R)}{ToHex(color.G)}{ToHex(color.B)}";
            else return $"#{ToHex(color.A)}{ToHex(color.R)}{ToHex(color.G)}{ToHex(color.B)}";
        }

        private static string ToHex(byte value)
        {
            return value.ToString("X2");
        }

#if LATER
        static double ParseColorValue(string elem, int maxValue, bool acceptPercent)
        {
            elem = elem.Trim();
            if (elem.EndsWith("%", StringComparison.Ordinal) && acceptPercent)
            {
                maxValue = 100;
                elem = elem.Substring(0, elem.Length - 1);
            }
            return (double)(int.Parse(elem, NumberStyles.Number, CultureInfo.InvariantCulture).Clamp(0, maxValue)) / maxValue;
        }

        static double ParseOpacity(string elem)
        {
            return double.Parse(elem, NumberStyles.Number, CultureInfo.InvariantCulture).Clamp(0, 1);
        }
#endif
    }

}
