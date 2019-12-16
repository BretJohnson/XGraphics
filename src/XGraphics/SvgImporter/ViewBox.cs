// This code is based on https://github.com/mono/SkiaSharp.Extended/blob/b93ad57152ad3d6274491d81fd38471e7d94f94b/SkiaSharp.Extended.Svg/source/SkiaSharp.Extended.Svg.Shared/ColorHelper.cs

using System;
using System.Collections.Generic;

namespace XGraphics.SvgImporter
{
	public class ViewBox
    {
        public Point Origin { get; }
        public Size Size { get; }

        public ViewBox(Point origin, Size size)
        {
            Origin = origin;
            Size = size;
        }
    }
}
