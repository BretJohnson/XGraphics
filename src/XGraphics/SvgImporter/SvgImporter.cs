// This code is based on https://github.com/mono/SkiaSharp.Extended/blob/b93ad57152ad3d6274491d81fd38471e7d94f94b/SkiaSharp.Extended.Svg/source/SkiaSharp.Extended.Svg.Shared/SKSvg.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using XGraphics.Brushes;
using XGraphics.Shapes;
using XGraphics.StandardModel;
using XGraphics.StandardModel.Brushes;
using XGraphics.StandardModel.Shapes;
using Path = System.IO.Path;

namespace XGraphics.SvgImporter
{
    public class SvgImporter
    {
        private const float DefaultPPI = 160f;

        private static readonly XNamespace XlinkNamespace = "http://www.w3.org/1999/xlink";
        private static readonly XNamespace SvgNamespace = "http://www.w3.org/2000/svg";
        private static readonly Regex UnitRegex = new Regex("px|pt|em|ex|pc|cm|mm|in");
        private static readonly Regex PercentRegex = new Regex("%");
        private static readonly Regex UrlRegex = new Regex(@"url\s*\(\s*#([^\)]+)\)");
        private static readonly Regex StyleKeyValueRegex = new Regex(@"\s*([\w-]+)\s*:\s*(.*)");

        private readonly Dictionary<string, Brush> _paintServers = new Dictionary<string, Brush>();
        private ViewBox? _viewBox = null;
        private string? _version;


        public float PixelsPerInch { get; set; }

        public XCanvas Import(Stream stream)
        {
            var nameTable = new NameTable();
            var xmlNamespaceManager = new XmlNamespaceManager(nameTable);
            xmlNamespaceManager.AddNamespace(string.Empty, SvgNamespace.NamespaceName);
            xmlNamespaceManager.AddNamespace("xlink", XlinkNamespace.NamespaceName);
            var xmlParserContext = new XmlParserContext(null, xmlNamespaceManager, null, XmlSpace.None);

            var xmlReaderSettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreComments = true,
            };

            using XmlReader reader = XmlReader.Create(stream, xmlReaderSettings, xmlParserContext);
            XDocument document = XDocument.Load(reader);
            return Import(document);
        }

        private XCanvas Import(XDocument document)
        {
            XElement svg = document.Root;
            XNamespace ns = svg.Name.Namespace;

            StyleableProperties svgStyleableProperties = ReadStyleableProperties(svg, null);

            // find the defs (gradients) - and follow all hrefs
            foreach (XElement element in svg.Descendants())
            {
                string? id = GetValue(element, "id");
                if (id != null)
                    ReadIdDefinition(element, id);
            }

            _version = GetValue(svg, "version");

            // TODO: parse the "preserveAspectRatio" values properly
            string? preserveAspectRatio = GetValue(svg, "preserveAspectRatio");

            _viewBox = ReadViewBox(svg, "viewBox");

            // get the user dimensions
            XAttribute widthA = svg.Attribute("width");
            XAttribute heightA = svg.Attribute("height");

            double width = HasValue(svg, "width")
                ? ReadHorizontalLengthOrPercentage(svg, "width")
                : _viewBox?.Size.Width ?? 0.0;
            double height = HasValue(svg, "height")
                ? ReadVerticalLengthOrPercentage(svg, "height")
                : _viewBox?.Size.Height ?? 0.0;

#if LATER
            // if there is no viewbox, then we don't do anything, otherwise
            // scale the SVG dimensions to fit inside the user dimensions
            if (!ViewBox.IsEmpty && (ViewBox.Width != CanvasSize.Width || ViewBox.Height != CanvasSize.Height))
            {
                if (preserveAspectRatio == "none")
                    canvas.Scale(CanvasSize.Width / ViewBox.Width, CanvasSize.Height / ViewBox.Height);
                else
                {
                    // TODO: just center scale for now
                    var scale = Math.Min(CanvasSize.Width / ViewBox.Width, CanvasSize.Height / ViewBox.Height);
                    var centered = SKRect.Create(CanvasSize).AspectFit(ViewBox.Size);
                    canvas.Translate(centered.Left, centered.Top);
                    canvas.Scale(scale, scale);
                }
            }

            // translate the canvas by the viewBox origin
            canvas.Translate(-ViewBox.Left, -ViewBox.Top);

            // if the viewbox was specified, then crop to that
            if (!ViewBox.IsEmpty)
                canvas.ClipRect(ViewBox);
#endif

            XCanvas xCanvas = new XCanvas();

            foreach (XElement e in svg.Elements())
            {
                GraphicsElement? graphicsElement = ReadElement(e, svgStyleableProperties);
                if (graphicsElement != null)
                    xCanvas.Children.Add(graphicsElement);
            }

            return xCanvas;
        }

        private GraphicsElement? ReadElement(XElement e, StyleableProperties parentStyleableProperties)
        {
            StyleableProperties styleableProperties = ReadStyleableProperties(e, parentStyleableProperties);

            string? display = GetValue(e, "display");
            if (display != null && display.Equals("none", StringComparison.Ordinal))
                return null;

#if LATER
            // transform matrix
            var transform = ReadTransform(e.Attribute("transform")?.Value ?? string.Empty);
            canvas.Save();
            canvas.Concat(ref transform);
#endif

#if LATER
            // clip-path
            var clipPath = ReadClipPath(e.Attribute("clip-path")?.Value ?? string.Empty);
            if (clipPath != null)
                canvas.ClipPath(clipPath);
#endif

            // SVG element
            string elementName = e.Name.LocalName;

            // parse elements
            switch (elementName)
            {
#if LATER
                case "image":
                    {
                        var image = ReadImage(e);
                        if (image.Bytes != null)
                        {
                            using (var bitmap = SKBitmap.Decode(image.Bytes))
                            {
                                if (bitmap != null)
                                    canvas.DrawBitmap(bitmap, image.Rect);
                            }
                        }
                    }
                    break;
                case "text":
                    if (stroke != null || fill != null)
                    {
                        var spans = ReadText(e, stroke?.Clone(), fill?.Clone());
                        if (spans.Any())
                        {
                            canvas.DrawText(spans);
                        }
                    }
                    break;
#endif
                case "rect":
                case "ellipse":
                case "circle":
                case "path":
                case "polygon":
                case "polyline":
                case "line":
                    Shape? shape = ReadShapeElement(e);
                    if (shape == null)
                        return null;

                    SetStrokeProperties(styleableProperties, shape);
                    SetFillProperties(styleableProperties, shape);
                    return shape;

#if LATER
                case "g":
                    if (e.HasElements)
                    {
                        // get current group opacity
                        float groupOpacity = ReadOpacity(style);
                        if (groupOpacity != 1.0f)
                        {
                            byte opacity = (byte)(255 * groupOpacity);
                            var opacityPaint = new SKPaint
                            {
                                Color = SKColors.Black.WithAlpha(opacity)
                            };

                            // apply the opacity
                            canvas.SaveLayer(opacityPaint);
                        }

                        foreach (XElement gElement in e.Elements())
                            ReadElement(gElement, stroke?.Clone(), fill?.Clone());

                        // restore state
                        if (groupOpacity != 1.0f)
                            canvas.Restore();
                    }
                    break;

                case "use":
                    if (e.HasAttributes)
                    {
                        var href = ReadHref(e);
                        if (href != null)
                        {
                            // create a deep copy as we will copy attributes
                            href = new XElement(href);
                            var attributes = e.Attributes();
                            foreach (var attribute in attributes)
                            {
                                var name = attribute.Name.LocalName;
                                if (!name.Equals("href", StringComparison.OrdinalIgnoreCase) &&
                                    !name.Equals("id", StringComparison.OrdinalIgnoreCase) &&
                                    !name.Equals("transform", StringComparison.OrdinalIgnoreCase))
                                {
                                    href.SetAttributeValue(attribute.Name, attribute.Value);
                                }
                            }

                            ReadElement(href, stroke?.Clone(), fill?.Clone());
                        }
                    }
                    break;

                case "switch":
                    if (e.HasElements)
                    {
                        foreach (var ee in e.Elements())
                        {
                            XAttribute requiredFeatures = ee.Attribute("requiredFeatures");
                            XAttribute requiredExtensions = ee.Attribute("requiredExtensions");
                            XAttribute systemLanguage = ee.Attribute("systemLanguage");

                            // TODO: evaluate requiredFeatures, requiredExtensions and systemLanguage
                            bool isVisible =
                                requiredFeatures == null &&
                                requiredExtensions == null &&
                                systemLanguage == null;

                            if (isVisible)
                                ReadElement(ee, stroke?.Clone(), fill?.Clone());
                        }
                    }
                    break;
#endif

                case "defs":
                case "title":
                case "desc":
                case "description":
                    // already read earlier
                    return null;

                default:
                    Log($"SVG element '{elementName}' is not supported");
                    return null;
            }
        }

        // TODO: Add Image support
#if LATER
        private SKSvgImage ReadImage(XElement e)
        {
            var x = ReadNumber(e.Attribute("x"));
            var y = ReadNumber(e.Attribute("y"));
            var width = ReadNumber(e.Attribute("width"));
            var height = ReadNumber(e.Attribute("height"));
            var rect = SKRect.Create(x, y, width, height);

            byte[] bytes = null;

            var uri = ReadHrefString(e);
            if (uri != null)
            {
                if (uri.StartsWith("data:"))
                {
                    bytes = ReadUriBytes(uri);
                }
                else
                {
                    LogOrThrow($"Remote images are not supported");
                }
            }

            return new SKSvgImage(rect, uri, bytes);
        }
#endif

        private Shape? ReadShapeElement(XElement e)
        {
            string elementName = e.Name.LocalName;
            switch (elementName)
            {
                case "rect":
                    return ReadRect(e);
                case "ellipse":
                    return ReadEllipse(e);
                case "circle":
                    return ReadCircle(e);
#if LATER
                case "path":
                case "polygon":
                case "polyline":
                    string? data = null;
                    if (elementName == "path")
                        data = e.Attribute("d")?.Value;
                    else
                    {
                        data = "M" + e.Attribute("points")?.Value;
                        if (elementName == "polygon")
                            data += " Z";
                    }

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        path.Dispose();
                        path = SKPath.ParseSvgPathData(data);
                    }
                    path.FillType = ReadFillRule(style);
                    break;
#endif
                case "line":
                    return ReadLine(e);
                default:
                    Log($"Unsupported shape type: '{elementName}'");
                    return null;
            }
        }

        private Ellipse ReadEllipse(XElement e)
        {
            double cx = ReadHorizontalLengthOrPercentage(e, "cx");
            double cy = ReadVerticalLengthOrPercentage(e, "cy");
            ReadRxRy(e, out double rx, out double ry);

            return new Ellipse
            {
                Left = cx - rx,
                Top = cy - ry,
                Width = 2 * rx,
                Height = 2 * ry
            };
        }

        private Ellipse ReadCircle(XElement e)
        {
            double cx = ReadHorizontalLengthOrPercentage(e, "cx");
            double cy = ReadVerticalLengthOrPercentage(e, "cy");
            double r = ReadDiagonalLengthOrPercentage(e, "r");

            return new Ellipse
            {
                Left = cx - r,
                Top = cy - r,
                Width = 2 * r,
                Height = 2 * r
            };
        }

        private Line ReadLine(XElement e)
        {
            double x1 = ReadHorizontalLengthOrPercentage(e, "x1");
            double x2 = ReadHorizontalLengthOrPercentage(e, "x2");
            double y1 = ReadVerticalLengthOrPercentage(e, "y1");
            double y2 = ReadVerticalLengthOrPercentage(e, "y2");

            return new Line
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2
            };
        }

        private Rectangle ReadRect(XElement e)
        {
            double x = ReadHorizontalLengthOrPercentage(e, "x");
            double y = ReadVerticalLengthOrPercentage(e, "y");
            double width = ReadHorizontalLengthOrPercentage(e, "width");
            double height = ReadVerticalLengthOrPercentage(e, "height");
            ReadRxRy(e, out double rx, out double ry);

            // Per SVG spec, clamp rx/ry to half the width/height
            if (rx > width / 2.0)
                rx = width / 2.0;
            if (ry > height / 2.0)
                ry = height / 2.0;

            return new Rectangle()
            {
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                RadiusX = rx,
                RadiusY = ry
            };
        }

        private void ReadRxRy(XElement e, out double rx, out double ry)
        {
            double? rxRaw = HasValue(e, "rx") ? ReadHorizontalLengthOrPercentage(e, "rx") : (double?) null;
            double? ryRaw = HasValue(e, "ry") ? ReadVerticalLengthOrPercentage(e, "ry") : (double?) null;

            // If rx isn't present default to ry and vice-versa
            rx = rxRaw ?? ryRaw ?? 0.0;
            ry = ryRaw ?? rxRaw ?? 0.0;
        }

        // TODO: Add Text support
#if LATER
        private SKText ReadText(XElement e, SKPaint stroke, SKPaint fill)
        {
            // TODO: stroke

            var x = ReadNumber(e.Attribute("x"));
            var y = ReadNumber(e.Attribute("y"));
            var xy = new SKPoint(x, y);
            var textAlign = ReadTextAlignment(e);
            var baselineShift = ReadBaselineShift(e);

            ReadFontAttributes(e, fill);

            return ReadTextSpans(e, xy, textAlign, baselineShift, stroke, fill);
        }

        private SKText ReadTextSpans(XElement e, SKPoint xy, SKTextAlign textAlign, float baselineShift, SKPaint stroke, SKPaint fill)
        {
            var spans = new SKText(xy, textAlign);

            // textAlign is used for all spans within the <text> element. If different textAligns would be needed, it is necessary to use
            // several <text> elements instead of <tspan> elements
            var currentBaselineShift = baselineShift;
            fill.TextAlign = SKTextAlign.Left;  // fixed alignment for all spans

            var nodes = e.Nodes().ToArray();
            for (int i = 0; i < nodes.Length; i++)
            {
                var c = nodes[i];
                bool isFirst = i == 0;
                bool isLast = i == nodes.Length - 1;

                if (c.NodeType == XmlNodeType.Text)
                {
                    // TODO: check for preserve whitespace

                    var textSegments = ((XText)c).Value.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    var count = textSegments.Length;
                    if (count > 0)
                    {
                        if (isFirst)
                            textSegments[0] = textSegments[0].TrimStart();
                        if (isLast)
                            textSegments[count - 1] = textSegments[count - 1].TrimEnd();
                        var text = WSRe.Replace(string.Concat(textSegments), " ");

                        spans.Append(new SKTextSpan(text, fill.Clone(), baselineShift: currentBaselineShift));
                    }
                }
                else if (c.NodeType == XmlNodeType.Element)
                {
                    var ce = (XElement)c;
                    if (ce.Name.LocalName == "tspan")
                    {
                        // the current span may want to change the cursor position
                        var x = ReadOptionalNumber(ce.Attribute("x"));
                        var y = ReadOptionalNumber(ce.Attribute("y"));
                        var text = ce.Value; //.Trim();

                        var spanFill = fill.Clone();
                        ReadFontAttributes(ce, spanFill);

                        // Don't read text-anchor from tspans!, Only use enclosing text-anchor from text element!
                        currentBaselineShift = ReadBaselineShift(ce);

                        spans.Append(new SKTextSpan(text, spanFill, x, y, currentBaselineShift));
                    }
                }
            }

            return spans;
        }

        private void ReadFontAttributes(XElement e, SKPaint paint)
        {
            Dictionary<string, string> fontStyle = ReadStyle(e);

            if (fontStyle == null || !fontStyle.TryGetValue("font-family", out string ffamily) || string.IsNullOrWhiteSpace(ffamily))
                ffamily = paint.Typeface?.FamilyName;
            int fweight = ReadFontWeight(fontStyle, paint.Typeface?.FontWeight ?? (int)SKFontStyleWeight.Normal);
            int fwidth = ReadFontWidth(fontStyle, paint.Typeface?.FontWidth ?? (int)SKFontStyleWidth.Normal);
            var fstyle = ReadFontStyle(fontStyle, paint.Typeface?.FontSlant ?? SKFontStyleSlant.Upright);

            paint.Typeface = SKTypeface.FromFamilyName(ffamily, fweight, fwidth, fstyle);

            if (fontStyle != null && fontStyle.TryGetValue("font-size", out string fsize) && !string.IsNullOrWhiteSpace(fsize))
                paint.TextSize = ReadNumber(fsize);
        }
#endif

#if LATER
        private static SKFontStyleSlant ReadFontStyle(Dictionary<string, string> fontStyle, SKFontStyleSlant defaultStyle = SKFontStyleSlant.Upright)
        {
            var style = defaultStyle;

            if (fontStyle != null && fontStyle.TryGetValue("font-style", out string fstyle) && !string.IsNullOrWhiteSpace(fstyle))
            {
                switch (fstyle)
                {
                    case "italic":
                        style = SKFontStyleSlant.Italic;
                        break;
                    case "oblique":
                        style = SKFontStyleSlant.Oblique;
                        break;
                    case "normal":
                        style = SKFontStyleSlant.Upright;
                        break;
                    default:
                        style = defaultStyle;
                        break;
                }
            }

            return style;
        }

        private int ReadFontWidth(Dictionary<string, string> fontStyle, int defaultWidth = (int)SKFontStyleWidth.Normal)
        {
            var width = defaultWidth;
            if (fontStyle != null && fontStyle.TryGetValue("font-stretch", out string fwidth) && !string.IsNullOrWhiteSpace(fwidth) && !int.TryParse(fwidth, out width))
            {
                switch (fwidth)
                {
                    case "ultra-condensed":
                        width = (int)SKFontStyleWidth.UltraCondensed;
                        break;
                    case "extra-condensed":
                        width = (int)SKFontStyleWidth.ExtraCondensed;
                        break;
                    case "condensed":
                        width = (int)SKFontStyleWidth.Condensed;
                        break;
                    case "semi-condensed":
                        width = (int)SKFontStyleWidth.SemiCondensed;
                        break;
                    case "normal":
                        width = (int)SKFontStyleWidth.Normal;
                        break;
                    case "semi-expanded":
                        width = (int)SKFontStyleWidth.SemiExpanded;
                        break;
                    case "expanded":
                        width = (int)SKFontStyleWidth.Expanded;
                        break;
                    case "extra-expanded":
                        width = (int)SKFontStyleWidth.ExtraExpanded;
                        break;
                    case "ultra-expanded":
                        width = (int)SKFontStyleWidth.UltraExpanded;
                        break;
                    case "wider":
                        width = width + 1;
                        break;
                    case "narrower":
                        width = width - 1;
                        break;
                    default:
                        width = defaultWidth;
                        break;
                }
            }

            return Math.Min(Math.Max((int)SKFontStyleWidth.UltraCondensed, width), (int)SKFontStyleWidth.UltraExpanded);
        }

        private int ReadFontWeight(Dictionary<string, string> fontStyle, int defaultWeight = (int)SKFontStyleWeight.Normal)
        {
            var weight = defaultWeight;

            if (fontStyle != null && fontStyle.TryGetValue("font-weight", out string fweight) && !string.IsNullOrWhiteSpace(fweight) && !int.TryParse(fweight, out weight))
            {
                switch (fweight)
                {
                    case "normal":
                        weight = (int)SKFontStyleWeight.Normal;
                        break;
                    case "bold":
                        weight = (int)SKFontStyleWeight.Bold;
                        break;
                    case "bolder":
                        weight = weight + 100;
                        break;
                    case "lighter":
                        weight = weight - 100;
                        break;
                    default:
                        weight = defaultWeight;
                        break;
                }
            }

            return Math.Min(Math.Max((int)SKFontStyleWeight.Thin, weight), (int)SKFontStyleWeight.ExtraBlack);
        }
#endif

        private void Log(string message)
        {
            Debug.WriteLine(message);
        }

        void LogInvalidValue(StyleableProperties styleableProperties, string property)
        {
            string? value = styleableProperties.GetValue(property);
            Log($"Invalid value for {property}: '{value}'");
        }

        void LogInvalidValue(XElement element, string attributeName)
        {
            string? value = GetValue(element, attributeName);
            Log($"Invalid value for {attributeName}: '{value}'");
        }

        private T LogAndUseDefault<T>(XElement element, string attributeName, T value)
        {
            LogInvalidValue(element, attributeName);
            return value;
        }

        private T LogAndUseDefault<T>(StyleableProperties styleableProperties, string property, T value)
        {
            LogInvalidValue(styleableProperties, property);
            return value;
        }

        private string GetString(Dictionary<string, string> style, string name, string defaultValue = "")
        {
            if (style != null && style.TryGetValue(name, out string v))
                return v;
            return defaultValue;
        }

        private StyleableProperties ReadStyleableProperties(XElement e, StyleableProperties? baseStyleableProperties)
        {
            // get from local attributes
            Dictionary<string, string> dictionary = e.Attributes().Where(a => HasSvgNamespace(a.Name)).ToDictionary(k => k.Name.LocalName, v => v.Value);

            string? style = GetValue(e, "style");
            if (style != null && !string.IsNullOrEmpty(style))
            {
                // get from style attribute
                Dictionary<string, string> styleDictionary = ReadStyleProperties(style);

                // overwrite, as style properties take precedence over separate attributes
                foreach (KeyValuePair<string, string> pair in styleDictionary)
                    dictionary[pair.Key] = pair.Value;
            }

            return new StyleableProperties(dictionary, baseStyleableProperties);
        }

        private Dictionary<string, string> ReadStyleProperties(string style)
        {
            var d = new Dictionary<string, string>();
            var kvs = style.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string kv in kvs)
            {
                Match m = StyleKeyValueRegex.Match(kv);
                if (m.Success)
                {
                    string k = m.Groups[1].Value;
                    string v = m.Groups[2].Value;
                    d[k] = v;
                }
            }
            return d;
        }

        private static bool HasSvgNamespace(XName name) =>
            string.IsNullOrEmpty(name.Namespace?.NamespaceName) ||
            name.Namespace == SvgNamespace ||
            name.Namespace == XlinkNamespace;

        private void SetStrokeProperties(StyleableProperties styleableProperties, Shape shape)
        {
            // TODO: Handle Opacity properly
#if LATER
            // get current element opacity, but ignore for groups (special case)
            double elementOpacity = isGroup ? 1.0 : ReadOpacity(styleableProperties);
#endif

            shape.Stroke = ReadPaint(styleableProperties, "stroke");
            if (shape.Stroke == null)
                return;

#if LATER
            // stroke attributes
            string strokeDashArray = styleableProperties.GetString("stroke-dasharray");
            bool hasStrokeDashArray = !string.IsNullOrWhiteSpace(strokeDashArray);


            string strokeOpacity = styleableProperties.GetString("stroke-opacity");
            bool hasStrokeOpacity = !string.IsNullOrWhiteSpace(strokeOpacity);
#endif

#if LATER
            if (hasStrokeDashArray)
            {
                if ("none".Equals(strokeDashArray, StringComparison.OrdinalIgnoreCase))
                {
                    // remove any dash
                    if (strokeInfo != null)
                        strokeInfo.PathEffect = null;
                }
                else
                {
                    // get the dash
                    var dashesStrings = strokeDashArray.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var dashes = dashesStrings.Select(ReadLength).ToArray();
                    if (dashesStrings.Length % 2 == 1)
                        dashes = dashes.Concat(dashes).ToArray();

                    // get the offset
                    var strokeDashOffset = ReadNumber(style, "stroke-dashoffset", 0);

                    // set the effect
                    strokeInfo.PathEffect = SKPathEffect.CreateDash(dashes.ToArray(), strokeDashOffset);
                }
            }
#endif

            shape.StrokeThickness = ReadDiagonalLengthOrPercentage(styleableProperties, "stroke-width", 1.0);

#if LATER
            if (hasStrokeOpacity)
                strokeInfo!.Color = strokeInfo.Color.WithA((byte)(ReadLength(strokeOpacity) * 255));
#endif
            shape.StrokeLineCap = ReadStrokeLineCap(styleableProperties, "stroke-linecap");
            shape.StrokeLineJoin = ReadStrokeLineJoin(styleableProperties, "stroke-linejoin");

            shape.StrokeMiterLimit = ReadNumber(styleableProperties, "stroke-miterlimit", 4.0);
#if LATER
            if (strokeInfo != null)
                strokeInfo!.Color = strokeInfo.Color.WithA((byte)(strokeInfo.Color.A * elementOpacity));
#endif
        }

        private void SetFillProperties(StyleableProperties styleableProperties, Shape shape)
        {
            shape.Fill = ReadPaint(styleableProperties, "fill");
            if (shape.Fill == null)
                return;

            // TODO: Include fill-rule for paths
#if LATER
            // fill attributes
            string fillOpacity = styleableProperties.GetString("fill-opacity");
            if (!string.IsNullOrWhiteSpace(fillOpacity))
            {
                if (fillInfo == null)
                    fillInfo = CreateFillInfo();

                fillInfo.Color = fillInfo.Color.WithA((byte)(ReadLength(fillOpacity) * 255));
            }

            if (fillInfo != null)
                fillInfo.Color = fillInfo.Color.WithA((byte)(fillInfo.Color.A * elementOpacity));
            }
#endif
        }

        private PenLineCap ReadStrokeLineCap(StyleableProperties styleableProperties, string property)
        {
            string? value = styleableProperties.GetValue(property);
            if (value == null)
                return PenLineCap.Flat;

            return value switch
            {
                "butt" => PenLineCap.Flat,
                "round" => PenLineCap.Round,
                "square" => PenLineCap.Square,
                _ => LogAndUseDefault(styleableProperties, property, PenLineCap.Flat)
            };
        }

        private PenLineJoin ReadStrokeLineJoin(StyleableProperties styleableProperties, string property)
        {
            string? value = styleableProperties.GetValue(property);
            if (value == null)
                return PenLineJoin.Miter;

            return value switch
            {
                // TODO: Handle miter-clip and arcs
                "miter" => PenLineJoin.Miter,
                "round" => PenLineJoin.Round,
                "bevel" => PenLineJoin.Bevel,
                _ => LogAndUseDefault(styleableProperties, property, PenLineJoin.Miter)
            };
        }

        private FillRule ReadFillRule(StyleableProperties styleableProperties, string property)
        {
            string? value = styleableProperties.GetValue(property);
            if (value == null)
                return FillRule.EvenOdd;

            return value switch
            {
                "evenodd" => FillRule.EvenOdd,
                "nonzero" => FillRule.Nonzero,
                _ => LogAndUseDefault(styleableProperties, property, FillRule.EvenOdd)
            };
        }

#if false
        private SKMatrix ReadTransform(string raw)
        {
            var t = SKMatrix.MakeIdentity();

            if (string.IsNullOrWhiteSpace(raw))
                return t;

            string[] calls = raw.Trim().Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in calls)
            {
                var args = c.Split(new[] { '(', ',', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var nt = SKMatrix.MakeIdentity();
                switch (args[0])
                {
                    case "matrix":
                        if (args.Length == 7)
                        {
                            nt.Values = new float[]
                            {
                                ReadNumber(args[1]), ReadNumber(args[3]), ReadNumber(args[5]),
                                ReadNumber(args[2]), ReadNumber(args[4]), ReadNumber(args[6]),
                                0, 0, 1
                            };
                        }
                        else
                        {
                            LogOrThrow($"Matrices are expected to have 6 elements, this one has {args.Length - 1}");
                        }
                        break;
                    case "translate":
                        if (args.Length >= 3)
                        {
                            nt = SKMatrix.MakeTranslation(ReadNumber(args[1]), ReadNumber(args[2]));
                        }
                        else if (args.Length >= 2)
                        {
                            nt = SKMatrix.MakeTranslation(ReadNumber(args[1]), 0);
                        }
                        break;
                    case "scale":
                        if (args.Length >= 3)
                        {
                            nt = SKMatrix.MakeScale(ReadNumber(args[1]), ReadNumber(args[2]));
                        }
                        else if (args.Length >= 2)
                        {
                            var sx = ReadNumber(args[1]);
                            nt = SKMatrix.MakeScale(sx, sx);
                        }
                        break;
                    case "rotate":
                        var a = ReadNumber(args[1]);
                        if (args.Length >= 4)
                        {
                            var x = ReadNumber(args[2]);
                            var y = ReadNumber(args[3]);
                            var t1 = SKMatrix.MakeTranslation(x, y);
                            var t2 = SKMatrix.MakeRotationDegrees(a);
                            var t3 = SKMatrix.MakeTranslation(-x, -y);
                            SKMatrix.Concat(ref nt, ref t1, ref t2);
                            SKMatrix.Concat(ref nt, ref nt, ref t3);
                        }
                        else
                        {
                            nt = SKMatrix.MakeRotationDegrees(a);
                        }
                        break;
                    default:
                        LogOrThrow($"Can't transform {args[0]}");
                        break;
                }
                SKMatrix.Concat(ref t, ref t, ref nt);
            }

            return t;
        }
#endif

#if LATER
        private SKPath ReadClipPath(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            SKPath result = null;
            var read = false;
            var urlM = urlRegex.Match(raw);
            if (urlM.Success)
            {
                var id = urlM.Groups[1].Value.Trim();

                if (_paintServers.TryGetValue(id, out XElement defE))
                {
                    result = ReadClipPathDefinition(defE);
                    if (result != null)
                    {
                        read = true;
                    }
                }
                else
                {
                    Log($"Invalid clip-path url reference: {id}");
                }
            }

            if (!read)
            {
                Log($"Unsupported clip-path: {raw}");
            }

            return result;
        }

        private SKPath ReadClipPathDefinition(XElement e)
        {
            if (e.Name.LocalName != "clipPath" || !e.HasElements)
            {
                return null;
            }

            var result = new SKPath();

            foreach (var ce in e.Elements())
            {
                var path = ReadShapeElement(ce);
                if (path != null)
                {
                    result.AddPath(path);
                }
                else
                {
                    Log($"SVG element '{ce.Name.LocalName}' is not supported in clipPath.");
                }
            }

            return result;
        }
#endif

#if LATER
        private SKTextAlign ReadTextAlignment(XElement element)
        {
            string value = null;
            if (element != null)
            {
                var attrib = element.Attribute("text-anchor");
                if (attrib != null && !string.IsNullOrWhiteSpace(attrib.Value))
                    value = attrib.Value;
                else
                {
                    var style = element.Attribute("style");
                    if (style != null && !string.IsNullOrWhiteSpace(style.Value))
                    {
                        value = GetString(ReadStyleAttribute(style.Value), "text-anchor");
                    }
                }
            }

            switch (value)
            {
                case "end":
                    return SKTextAlign.Right;
                case "middle":
                    return SKTextAlign.Center;
                default:
                    return SKTextAlign.Left;
            }
        }

        private float ReadBaselineShift(XElement element)
        {
            string value = null;
            if (element != null)
            {
                var attrib = element.Attribute("baseline-shift");
                if (attrib != null && !string.IsNullOrWhiteSpace(attrib.Value))
                    value = attrib.Value;
                else
                {
                    var style = element.Attribute("style");
                    if (style != null && !string.IsNullOrWhiteSpace(style.Value))
                    {
                        value = GetString(ReadStyleAttribute(style.Value), "baseline-shift");
                    }
                }
            }

            return ReadLength(value);
        }
#endif

        private void ReadIdDefinition(XElement e, string id)
        {
            string elementName = e.Name.LocalName.ToLower(CultureInfo.InvariantCulture);

            switch (elementName)
            {
                case "lineargradient":
                    _paintServers[id] = ReadLinearGradient(e);
                    break;

                case "radialgradient":
                    _paintServers[id] = ReadRadialGradient(e);
                    break;
            }
        }

        private LinearGradientBrush ReadLinearGradient(XElement e)
        {
            var start = new Point(
                ReadLengthOrPercentage(e, "x1", 0.0, 1.0),
                ReadLengthOrPercentage(e, "y1", 0.0, 1.0));
            var end = new Point(
                ReadLengthOrPercentage(e, "x2", 1.0, 1.0),
                ReadLengthOrPercentage(e, "y2", 0.0, 1.0));

            ReadAndValidateGradientUnits(e, "gradientUnits");
            GradientSpreadMethod spreadMethod = ReadSpreadMethod(e, "spreadMethod");
            XGraphicsCollection<GradientStop> stops = ReadGradientStops(e);

#if LATER
            var matrix = ReadTransform(e.Attribute("gradientTransform")?.Value ?? string.Empty);
#endif

            return new LinearGradientBrush
            {
                StartPoint = start,
                EndPoint = end,
                MappingMode = BrushMappingMode.RelativeToBoundingBox,
                GradientStops = stops,
                SpreadMethod = spreadMethod
            };
        }

        private RadialGradientBrush ReadRadialGradient(XElement e)
        {
            var center = new Point(
                ReadLengthOrPercentage(e, "cx", 0.5, 1.0),
                ReadLengthOrPercentage(e, "cy", 0.5, 1.0));
            double radius = ReadLengthOrPercentage(e, "r", 0.5, 1.0);

#if LATER
            //var focusX = ReadOptionalNumber(e.Attribute("fx")) ?? centerX;
            //var focusY = ReadOptionalNumber(e.Attribute("fy")) ?? centerY;
            //var absolute = e.Attribute("gradientUnits")?.Value == "userSpaceOnUse";
#endif

            ReadAndValidateGradientUnits(e, "gradientUnits");
            GradientSpreadMethod spreadMethod = ReadSpreadMethod(e, "spreadMethod");
            XGraphicsCollection<GradientStop> stops = ReadGradientStops(e);

#if LATER
            var matrix = ReadTransform(e.Attribute("gradientTransform")?.Value ?? string.Empty);
#endif

            return new RadialGradientBrush
            {
                Center = center,
                RadiusX = radius,
                MappingMode = BrushMappingMode.RelativeToBoundingBox,
                GradientStops = stops,
                SpreadMethod = spreadMethod
            };
        }

        private void ReadAndValidateGradientUnits(XElement e, string attributeName)
        {
            // TODO: Support MappingMode

            // Only the default "objectBoundingBox" is currently supported for gradientUnits; log a message if it's not that
            string? raw = GetValue(e, attributeName);
            if (raw != null && raw != "objectBoundingBox")
                LogInvalidValue(e, attributeName);
        }

        private GradientSpreadMethod ReadSpreadMethod(XElement e, string attributeName)
        {
            string? spreadMethod = GetValue(e, attributeName);
            if (spreadMethod == null)
                return GradientSpreadMethod.Pad;

            return spreadMethod switch
            {
                "reflect" => GradientSpreadMethod.Reflect,
                "repeat" => GradientSpreadMethod.Repeat,
                "pad" => GradientSpreadMethod.Pad,
                _ => LogAndUseDefault(e, attributeName, GradientSpreadMethod.Pad)
            };
        }

        private XGraphicsCollection<GradientStop> ReadGradientStops(XElement e)
        {
            var gradientStops = new XGraphicsCollection<GradientStop>();

            XNamespace ns = e.Name.Namespace;
            foreach (XElement stopElement in e.Elements(ns + "stop"))
            {
                StyleableProperties styleableProperties = ReadStyleableProperties(stopElement, null);

                double offset = ReadNumberOrPercentage(styleableProperties, "offset", 0.0, 1.0);

                // Clamp offset to be between 0.0 and 1.0
                if (offset < 0.0)
                    offset = 0.0;
                else if (offset > 1.0)
                    offset = 1.0;

                if (gradientStops.Count > 0)
                {
                    // Per SVG spec, force offset to be >= all previous offsets
                    double previousOffset = gradientStops[gradientStops.Count - 1].Offset;
                    if (offset < previousOffset)
                        offset = previousOffset;
                }

                // TODO: Support this from the SVG spec: "If two gradient stops have the same offset value, then the latter gradient stop controls the color value at the overlap point"

                Color stopColor;
                // Per the SVG spec, treat "transparent" for stop-color as transparent black
                if (styleableProperties.GetValue("stop-color") == "transparent")
                    stopColor = new Color(0, 0, 0, 0);
                else stopColor = ReadColor(styleableProperties, "stop-color", Colors.Black);

                double opacity = ReadAlphaValue(styleableProperties, "stop-opacity");
                double aggregateOpacity = (stopColor.A / 255.0) * opacity;
                stopColor = stopColor.WithA((byte)(aggregateOpacity * 255.0));

                var gradientStop = new GradientStop() { Offset = offset, Color = stopColor };
                gradientStops.Add(gradientStop);
            }

            return gradientStops;
        }

        private double ReadNumberOrPercentage(StyleableProperties styleableProperties, string property, double defaultValue, double maxForPercentage)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return defaultValue;

            double multiplier = 1.0;
            if (PercentRegex.IsMatch(raw))
            {
                raw = raw.Substring(0, raw.Length - 1);
                multiplier = maxForPercentage / 100.0;
            }

            if (!TryParseNumber(raw, out double value))
                return LogAndUseDefault(styleableProperties, property, defaultValue);

            return value * multiplier;
        }

        private double ReadOpacity(StyleableProperties styleableProperties) =>
            ReadAlphaValue(styleableProperties, "opacity");

        private byte[]? ReadUriBytes(string uri)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                int offset = uri.IndexOf(",", StringComparison.Ordinal);
                if (offset != -1 && offset - 1 < uri.Length)
                {
                    uri = uri.Substring(offset + 1);
                    return Convert.FromBase64String(uri);
                }
            }

            return null;
        }

        private ViewBox? ReadViewBox(XElement element, string attributeName)
        {
            string? raw = GetValue(element, attributeName);
            if (raw == null)
                return null;

            bool error = false;
            double originX = 0.0, originY = 0.0, width = 0.0, height = 0.0;

            // Allow comma or whitespace as delimiters
            raw = raw.Replace(',', ' ');
            string[] p = raw.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (p.Length == 4)
            {
                if (!TryParseNumber(p[0], out originX))
                    error = true;
                if (!TryParseNumber(p[1], out originY))
                    error = true;
                if (!TryParseNumber(p[2], out width))
                    error = true;
                if (!TryParseNumber(p[3], out height))
                    error = true;
            }
            else error = true;

            if (error)
            {
                LogInvalidValue(element, attributeName);
                return null;
            }

            return new ViewBox(new Point(originX, originY), new Size(width, height));
        }

        private double ReadAlphaValue(StyleableProperties styleableProperties, string property)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return 1.0;

            double m = 1.0;
            if (PercentRegex.IsMatch(raw))
            {
                raw = raw.Substring(0, raw.Length - 1);
                m = 0.01;
            }

            if (!TryParseNumber(raw, out double v))
                return LogAndUseDefault(styleableProperties, property, 1.0);

            double value = m * v;

            // Clamp to be between 0.0 and 1.0
            if (value < 0.0)
                value = 0.0;
            if (value > 1.0)
                value = 1.0;

            return value;
        }

        private Brush? ReadPaint(StyleableProperties styleableProperties, string property)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return null;

            if (raw.Equals("none", StringComparison.OrdinalIgnoreCase))
                return null;

            if (ColorHelper.TryParse(raw, out Color color))
                return new SolidColorBrush {Color = color};

            Match urlMatch = UrlRegex.Match(raw);
            if (urlMatch.Success)
            {
                string id = urlMatch.Groups[1].Value.Trim();
                if (_paintServers.TryGetValue(id, out Brush brush))
                    return brush;
                else
                {
                    Log($"Invalid url id reference: {id}");
                    return null;
                }
            }

            Log($"Invalid paint: '{raw}'");
            return null;
        }

        private Color ReadColor(StyleableProperties styleableProperties, string property, Color defaultValue)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return defaultValue;

            // TODO: Support color=transparent
            if (ColorHelper.TryParse(raw, out Color color))
                return color;

            return LogAndUseDefault(styleableProperties, property, defaultValue);
        }

        private double ReadNumber(StyleableProperties styleableProperties, string property, double defaultValue)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return defaultValue;

            if (!TryParseNumber(raw, out double value))
            {
                Log($"Invalid number: '{raw}'");
                return defaultValue;
            }

            return value;
        }

        private double ReadLengthOrPercentage(XElement element, string attributeName, double defaultValue, double maxForPercentage)
        {
            string? raw = GetValue(element, attributeName);
            if (raw == null)
                return defaultValue;

            double multiplier = ExtractLengthMultiplier(maxForPercentage, ref raw);

            if (!TryParseNumber(raw, out double value))
                return LogAndUseDefault(element, attributeName, defaultValue);

            return multiplier * value;
        }

        private double ReadLengthOrPercentage(StyleableProperties styleableProperties, string property, double defaultValue, double maxForPercentage)
        {
            string? raw = styleableProperties.GetValue(property);
            if (raw == null)
                return defaultValue;

            double multiplier = ExtractLengthMultiplier(maxForPercentage, ref raw);

            if (!TryParseNumber(raw, out double value))
                return LogAndUseDefault(styleableProperties, property, defaultValue);

            return multiplier * value
                ;
        }

        private double ExtractLengthMultiplier(double maxForPercentage, ref string raw)
        {
            double multiplier = 1.0;
            if (UnitRegex.IsMatch(raw))
            {
                if (raw.EndsWith("in", StringComparison.Ordinal))
                    multiplier = PixelsPerInch;
                else if (raw.EndsWith("cm", StringComparison.Ordinal))
                    multiplier = PixelsPerInch / 2.54;
                else if (raw.EndsWith("mm", StringComparison.Ordinal))
                    multiplier = PixelsPerInch / 25.4;
                else if (raw.EndsWith("pt", StringComparison.Ordinal))
                    multiplier = PixelsPerInch / 72.0;
                else if (raw.EndsWith("pc", StringComparison.Ordinal))
                    multiplier = PixelsPerInch / 6.0;
                raw = raw.Substring(0, raw.Length - 2);
            }
            else if (PercentRegex.IsMatch(raw))
            {
                // TODO: Support percentages properly, with the viewport size
                raw = raw.Substring(0, raw.Length - 1);
                multiplier = maxForPercentage / 100.0;
            }

            return multiplier;
        }

        private double ReadHorizontalLengthOrPercentage(XElement element, string attributeName)
        {
            return ReadLengthOrPercentage(element, attributeName, 0.0, 100);
        }

        private double ReadVerticalLengthOrPercentage(XElement element, string attributeName)
        {
            return ReadLengthOrPercentage(element, attributeName, 0.0, 100);
        }

        private double ReadDiagonalLengthOrPercentage(XElement element, string attributeName)
        {
            return ReadLengthOrPercentage(element, attributeName, 0.0, 100);
        }

        private double ReadDiagonalLengthOrPercentage(StyleableProperties styleableProperties, string property, double defaultValue)
        {
            return ReadLengthOrPercentage(styleableProperties, property, defaultValue, 100);
        }

        private string? GetValue(XElement element, string attributeName) =>
            element.Attribute(attributeName)?.Value.Trim();

        private bool HasValue(XElement element, string attributeName) =>
            element.Attribute(attributeName) != null;

        private bool TryParseNumber(string raw, out double value)
        {
            return double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}
