using SkiaSharp;

namespace XGraphics.SkiaRenderer
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
