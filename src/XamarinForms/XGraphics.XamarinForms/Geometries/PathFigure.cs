// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PathFigure : BindableObjectWithCascadingNotifications, IPathFigure
    {
        public static readonly BindableProperty SegmentsProperty = PropertyUtils.Create(nameof(Segments), typeof(XGraphicsCollection<PathSegment>), typeof(PathFigure), null);
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(Wrapper.Point), typeof(PathFigure), Wrapper.Point.Default);
        public static readonly BindableProperty IsClosedProperty = PropertyUtils.Create(nameof(IsClosed), typeof(bool), typeof(PathFigure), false);
        public static readonly BindableProperty IsFilledProperty = PropertyUtils.Create(nameof(IsFilled), typeof(bool), typeof(PathFigure), true);

        public PathFigure()
        {
            Segments = new XGraphicsCollection<PathSegment>();
        }

        public XGraphicsCollection<PathSegment> Segments
        {
            get => (XGraphicsCollection<PathSegment>)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }
        IEnumerable<IPathSegment> IPathFigure.Segments => Segments;

        public Wrapper.Point StartPoint
        {
            get => (Wrapper.Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point IPathFigure.StartPoint => StartPoint.WrappedPoint;

        public bool IsClosed
        {
            get => (bool)GetValue(IsClosedProperty);
            set => SetValue(IsClosedProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, value);
        }
    }
}