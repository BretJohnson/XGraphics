using System;

namespace XGraphics
{
    public abstract class Canvas
    {
        public virtual void DrawShape(Shape shape)
        {
            if (shape is Rectangle rectangle)
                DrawRectangle(rectangle);
            else throw new InvalidOperationException($"Unknown shape type {shape.GetType()}");
        }

        public abstract void DrawRectangle(Rectangle rectangle);
    }
}
