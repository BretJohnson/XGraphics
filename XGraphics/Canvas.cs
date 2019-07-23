using System;
using XGraphics.Shapes;

namespace XGraphics
{
    public abstract class Canvas
    {
        public virtual void DrawShape(IShape shape)
        {
            if (shape is IRectangle rectangle)
                DrawRectangle(rectangle);
            else throw new InvalidOperationException($"Unknown shape type {shape.GetType()}");
        }

        public abstract void DrawRectangle(IRectangle rectangle);
    }
}
