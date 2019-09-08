namespace XGraphics
{
    public struct Color
    {
        public byte A { get; set; }

        public byte B { get; set; }

        public byte G { get; set; }

        public byte R { get; set; }

        public static Color FromArgb(byte a, byte r, byte g, byte b) =>
            new Color()
            {
                A = a,
                R = r,
                G = g,
                B = b
            };

        public static Color FromRgb(byte r, byte g, byte b)
        {
            return FromArgb(255, r, g, b);
        }

        public static Color FromHex(string hex)
        {
            // Undefined
            if (hex.Length < 3)
                return Colors.Transparent;
            int idx = (hex[0] == '#') ? 1 : 0;

            switch (hex.Length - idx)
            {
                case 3: //#rgb => ffrrggbb
                    var t1 = ToHexD(hex[idx++]);
                    var t2 = ToHexD(hex[idx++]);
                    var t3 = ToHexD(hex[idx]);

                    return FromRgb(t1, t2, t3);

                case 4: //#argb => aarrggbb
                    var f1 = ToHexD(hex[idx++]);
                    var f2 = ToHexD(hex[idx++]);
                    var f3 = ToHexD(hex[idx++]);
                    var f4 = ToHexD(hex[idx]);
                    return FromArgb(f1, f2, f3, f4);

                case 6: //#rrggbb => ffrrggbb
                    return FromRgb(
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])));

                case 8: //#aarrggbb
                    var a1 = (byte) (ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]));
                    return FromArgb(
                        a1,
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (byte)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])));

                default: //everything else will result in unexpected results
                    return Colors.Transparent;
            }
        }

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
            var j = ToHex(c);
            return (byte)((j << 4) | j);
        }

    }
}
