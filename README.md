# XGraphics: XAML-standard, animatable graphics, for all platforms [![Gitter](https://badges.gitter.im/XGraphics/community.svg)](https://gitter.im/XGraphics/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

XGraphics aims to solve these problems:

1. **Bring XAML-based graphics to Xamarin.Forms**

   Today, graphics in Xamarin.Forms are typically implemented with in SkiaSharp, from C# code.

   XGraphics lets you instead do that directly from XAML, in the same way you can in UWP/WPF.
   That's more concise and allows use of Hot Reload to see updates immediately as you get your drawn UI to look just right. It also allows use for XAML animations, for sophisticated visual effects.

2. **Be the XAML standard for graphics shape elements (ellipses, paths, gradients, transformations, etc).**
 
   Today UWP, WPF, and Silverlight all use very similar, but slightly different, graphics elements.
   Still, those form something of a standard, with many design tools and icon libraries today supporting XAML
   vector graphics, in much the same way they support SVG.

   XGraphics takes the de facto standard UWP/WPF/Sliverlight graphics model and brings them to all platforms.
   So you do can take XAML like what's below and use it on any XAML platform - Xamarin.Forms, UWP, WPF, or (eventually) community XAML based platforms like Uno, Ooui, and Avalonia UI. We may not have a XAML Standard for everything, but having a standard for graphic elements is an important step. A standard also helps design assets and graphics tools (e.g. https://github.com/netonjm/FigmaSharp) work across all platforms.

   ```
        <XCanvas x:Name="arrow_corner_up_right" xmlns="http://schemas.microsoft.com/xgraphics/2019">
            <Path Width="40.25" Height="40" Left="22" Top="19" Data="F1 M 22,30L 47.75,30L 36.75,19L 48.25,19L 62.25,33L 47.25,48L 36.75,48L 47.75,37L 29,37L 29,59L 22,59L 22,30 Z">
                <Path.Fill>
                    <SolidColorBrush Color="Blue" />
                </Path.Fill>
                <Path.RenderTransform>
                    <TransformGroup>
                        <TranslateTransform X="200" Y="200"/>
                        <ScaleTransform ScaleX="2" ScaleY="2" />
                    </TransformGroup>
                </Path.RenderTransform>
            </Path>
        </XCanvas>
    ```

<img style="padding-left:40px" src="Media/arrow-corner-up-right.png">

3. **Support pluggable renderer backends, to work well everywhere**

   By default XGraphics uses SkiaSharp to do its rendering. SkiaSharp is nice as it supports pixel perfect identical rendering, on the many Skia supported platforms, with OpenGL acceleration.

   SkiaSharp, with all its powerful goodness, is a little hefty though - it brings about 5MB additional code size to mobile apps - so some prefer not to use it,
   especially when the graphics needs are very simple, say just drawing an ellipse.

   To address this, XGraphics supports pluggable renderers, including renders that use the underlying native platform graphics APIs (much like https://github.com/praeclarum/NGraphics). Using the native renderer won't give you pixel perfect consistency, nor some of the more advanced Skia features, but you'll get a leaner
   app in return. The nice thing about the XGraphics approach is that you can use the same graphics primitives everywhere and then decide on a platform by
   platform basis if you want to use the Skia renderer or the native one, changing with just the flick of a switch.

4. **Support more than just XAML markup**

   XGraphics is really a standard graphics object model, not locked to XAML specifically. It will support (coming soon) a straight C# flavor for all the graphics elements, in addition to their XAML framework versions. Using the C# flavor is appropriate for apps that don't use XAML at all (say because they use coded UI
   or Xamarin Android/iOS native, not Forms, UI).

   This is all supported by having shared interfaces, like `XGraphics.IEllipse`, which define the object model. Then there are concrete versions of the objects,
   like `XGraphics.XamarinForms.Ellipse` and `XGraphics.WPF.Ellipse` and which implement the interface is the appropriate way for the given XAML flavor or plain old C# object. You don't need to worry about this implementation detail, but Roslyn based code gen creates the concrete classes. Similar technology could be used other places to create a "XAML standard" object model that works across all flavors of XAML, with shared code.

# Currently Supported Elements

### Shape elements

| Element     | Notes, including any differences from UWP |
| ----------- | ----------- |
| [`XCanvas`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.canvas)      | root Canvas; renamed to identify clearly as XGraphics |
| [`Canvas`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.canvas)   | child canvas |
| [`Shape`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.shape)  | `Stretch` property not yet implemented |
| [`Ellipse`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.ellipse)  |  |
| [`Line`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.line)  | |
| [`Rectangle`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.rectangle)  | |
| [`Polygon`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.polygon)  | |
| [`Polyline`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.polyline)  | |
| [`Path`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.shapes.path)  | |

### Geometry elements (mainly used for paths)
| Element | Notes, including any differences from UWP |
| ----------- | ----------- |
| [`Geometry`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.geometry) | Base class for geometries |
| [`PathGeometry`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.pathgeometry)  | Contains multiple `PathFigures` |
| [`PathFigure`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.pathfigure)   | Contains multiple `PathSegments` |
| [`PathSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.pathsegment)   | Base class for different path segment types |
| [`LineSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.linesegment)   | |
| [`ArcSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.arcsegment) | |
| [`BezierSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.beziersegment)   | |
| [`PolyBezierSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.polybeziersegment)   | |
| [`PolyLineSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.polylinesegment)   | |
| [`PolyQuadraticBezierSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.polyquadraticbeziersegment)   | |
| [`QuadraticBezierSegment`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.quadraticbeziersegment) | |

### Transform elements
| Element | Notes, including any differences from UWP |
| ----------- | ----------- |
| [`Transform`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.transform) | Transform base class |
| [`RotateTransform`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.rotatetransform)  | |
| [`ScaleTransform`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.scaletransform)   | |
| [`TranslateTransform`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.translatetransform)   | |
| [`TransformGroup`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.transformgroup)   | |

### Brush elements
| Element | Notes, including any differences from UWP |
| ----------- | ----------- |
| [`Brush`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) | `Opacity` and transform properties not yet implemented |
| [`SolidColorBrush`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.solidcolorbrush)  | |
| [`GradientBrush`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.gradientbrush)   | |
| [`GradientStop`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.gradientstop)   | |
| [`LinearGradientBrush`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.lineargradientbrush)   | |
| [`RadialGradientBrush`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.radialgradientbrush?view=netframework-4.8)   | Not in standard UWP, but exists in WPF and Windows community toolkit. XGraphics only supports circular, not elliptical, gradients, so it just has a single `Radius` property |

