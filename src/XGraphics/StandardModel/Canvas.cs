// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Brushes;
using XGraphics.StandardModel.Brushes;
using XGraphics;

namespace XGraphics.StandardModel
{
    public class Canvas : GraphicsElement, ICanvas
    {
        public Canvas()
        {
            Children = new XGraphicsCollection<GraphicsElement>();
        }

        public XGraphicsCollection<GraphicsElement> Children { get; set; } = null;

        IEnumerable<IGraphicsElement> ICanvas.Children => Children;

        public Brush? Background { get; set; } = null;

        IBrush? ICanvas.Background => Background;
    }
}