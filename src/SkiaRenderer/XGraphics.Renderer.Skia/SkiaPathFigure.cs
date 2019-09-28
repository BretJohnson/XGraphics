using SkiaSharp;

namespace XGraphics.Renderer.Skia
{
    public struct SkiaPathFigure
    {
        public SKPath Path { get; }
        public bool IsFilled { get; }

        public SkiaPathFigure(SKPath path, bool isFilled)
        {
            Path = path;
            IsFilled = isFilled;
        }
    }
}
