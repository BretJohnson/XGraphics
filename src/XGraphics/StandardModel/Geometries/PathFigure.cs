// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Geometries;

namespace XGraphics.StandardModel.Geometries
{
    public class PathFigure : ObjectWithCascadingNotifications, IPathFigure
    {
        public PathFigure()
        {
            Segments = new XGraphicsCollection<PathSegment>();
        }

        public XGraphicsCollection<PathSegment> Segments { get; set; } = null;

        IEnumerable<IPathSegment> IPathFigure.Segments => Segments;

        public Point StartPoint { get; set; } = Point.Default;

        public bool IsClosed { get; set; } = false;

        public bool IsFilled { get; set; } = true;
    }
}