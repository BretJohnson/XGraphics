using System.Collections.Generic;
using System.Windows;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using XGraphics.Skia;

namespace XGraphics.Example.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            OnPaintSurface(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        private void OnPaintSurface(SKCanvas skCanvas, int width, int height)
        {
            var skiaCanvas = new SkiaCanvas(skCanvas);

            foreach (Shape shape in SampleShapes())
                skiaCanvas.DrawShape(shape);
        }

        private IEnumerable<Shape> SampleShapes()
        {
            Color red = Color.FromArgb(255, 255, 0, 0);
            Color blue = Color.FromArgb(255, 0, 0, 255);

            yield return new Rectangle()
            {
                Left = 0,
                Top = 0,
                Width = 50,
                Height = 50,
                Stroke = new SolidColorBrush(red),
                StrokeThickness = 2,
                Fill = new SolidColorBrush(blue)
            };

            yield return new Rectangle()
            {
                Left = 50,
                Top = 50,
                Width = 100,
                Height = 100,
                Stroke = new SolidColorBrush(blue),
                StrokeThickness = 5,
                Fill = new SolidColorBrush(red)
            };
        }

        private void DrawXamagon(SKCanvas canvas)
        {
            // clear the canvas / fill with white
            canvas.Clear(SKColors.White);

            // set up drawing tools
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(0x2c, 0x3e, 0x50);
                paint.StrokeCap = SKStrokeCap.Round;

                // create the Xamagon path
                using (var path = new SKPath())
                {
                    path.MoveTo(71.4311121f, 56f);
                    path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
                    path.LineTo(43.0238921f, 97.5342563f);
                    path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
                    path.LineTo(64.5928855f, 143.034271f);
                    path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
                    path.LineTo(114.568946f, 147f);
                    path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
                    path.LineTo(142.976161f, 105.465744f);
                    path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
                    path.LineTo(121.407172f, 59.965729f);
                    path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
                    path.LineTo(71.4311121f, 56f);
                    path.Close();

                    // draw the Xamagon path
                    canvas.DrawPath(path, paint);
                }
            }
        }
    }
}
