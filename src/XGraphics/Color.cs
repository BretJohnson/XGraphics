namespace XGraphics
{
    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// </summary>
    public struct Color
    {
        public static readonly Color Default = new Color(0, 0, 0, 0);

        /// <summary>
        /// Gets the sRGB alpha channel value of the color.
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Gets the sRGB blue channel value of the color.
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the sRGB green channel value of the color.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the sRGB red channel value of the color.
        /// </summary>
        public byte R { get; }

        public static Color FromArgb(byte a, byte r, byte g, byte b) => new Color(a, r, g, b);

        public static Color FromRgb(byte r, byte g, byte b) => FromArgb(255, r, g, b);

        public static bool FromHex(string hex, out Color color)
        {
            color = Color.Default;

            if (hex.Length < 3)
                return false;

            int idx = (hex[0] == '#') ? 1 : 0;

            switch (hex.Length - idx)
            {
                case 3: //#rgb => ffrrggbb
                    byte t1 = ToHexD(hex[idx++]);
                    byte t2 = ToHexD(hex[idx++]);
                    byte t3 = ToHexD(hex[idx]);
                    color = FromRgb(t1, t2, t3);
                    return true;

                case 4: //#argb => aarrggbb
                    byte f1 = ToHexD(hex[idx++]);
                    byte f2 = ToHexD(hex[idx++]);
                    byte f3 = ToHexD(hex[idx++]);
                    byte f4 = ToHexD(hex[idx]);
                    color = FromArgb(f1, f2, f3, f4);
                    return true;

                case 6: //#rrggbb => ffrrggbb
                    color = FromRgb(
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])));
                    return true;

                case 8: //#aarrggbb
                    byte a1 = (byte) (ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]));
                    color = FromArgb(
                        a1,
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])));
                    return true;

                default: //everything else will result in unexpected results
                    return false;
            }
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Color WithA(byte a) => new Color(a, R, G, B);

        public Color WithR(byte r) => new Color(A, r, G, B);

        public Color WithG(byte g) => new Color(A, R, g, B);

        public Color WithB(byte b) => new Color(A, R, G, b);

        private static byte ToHex(char c)
        {
            ushort x = (ushort)c;
            if (x >= '0' && x <= '9')
                return (byte)(x - '0');

            x |= 0x20;
            if (x >= 'a' && x <= 'f')
                return (byte)(x - 'a' + 10);
            return 0;
        }

        private static byte ToHexD(char c)
        {
            byte j = ToHex(c);
            return (byte)((j << 4) | j);
        }
    }
}
