// Licensed to the .NET Foundation under one or more agreements.

// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// This class is used by the StreamGeometry class to generate an inlined,
// flattened geometry stream.
//

using System.Collections.Generic;
using XGraphics.Geometries;

namespace XGraphics.Converters.Path
{
    internal abstract class StreamGeometryContext
    {
        /// <summary>
        /// BeginFigure - Start a new figure.
        /// </summary>
        public abstract void BeginFigure(Point startPoint, bool isFilled, bool isClosed);
        
        /// <summary>
        /// LineTo - append a LineTo to the current figure.
        /// </summary>
        public abstract void LineTo(Point point);

        /// <summary>
        /// QuadraticBezierTo - append a QuadraticBezierTo to the current figure.
        /// </summary>
        public abstract void QuadraticBezierTo(Point point1, Point point2);
        
        /// <summary>
        /// BezierTo - apply a BezierTo to the current figure.
        /// </summary>
        public abstract void BezierTo(Point point1, Point point2, Point point3);
        
        /// <summary>
        /// PolyLineTo - append a PolyLineTo to the current figure.
        /// </summary>
        public abstract void PolyLineTo(IList<Point> points);

        /// <summary>
        /// PolyQuadraticBezierTo - append a PolyQuadraticBezierTo to the current figure.
        /// </summary>
        public abstract void PolyQuadraticBezierTo(IList<Point> points);

        /// <summary>
        /// PolyBezierTo - append a PolyBezierTo to the current figure.
        /// </summary>
        public abstract void PolyBezierTo(IList<Point> points);

        /// <summary>
        /// ArcTo - append an ArcTo to the current figure.
        /// </summary>

        public abstract void ArcTo(Point point, Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection);

        /// <summary>
        /// SetClosedState - Sets the current closed state of the figure. 
        /// </summary>
        internal abstract void SetClosedState(bool closed);
    }
}
