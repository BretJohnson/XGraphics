using System.Collections.Generic;
using XGraphics.Geometries;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Geometries
{
    public class PathFigure : BindableObjectWithCascadingNotifications, IPathFigure
    {
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(Wrapper.Point), typeof(PathFigure), PropertyUtils.DefaultPoint);
        public static readonly BindableProperty IsClosedProperty = PropertyUtils.Create(nameof(IsClosed), typeof(bool), typeof(PathFigure), false);
        public static readonly BindableProperty IsFilledProperty = PropertyUtils.Create(nameof(IsFilled), typeof(bool), typeof(PathFigure), true);

        public PathFigure()
        {
            Segments = new GraphicsObjectCollection<PathSegment>();
            Segments.Changed += OnSubobjectChanged;
        }

        IEnumerable<IPathSegment> IPathFigure.Segments => Segments;
        public GraphicsObjectCollection<PathSegment> Segments
        {
            get;
        }

        Point IPathFigure.StartPoint => StartPoint.WrappedPoint;
        public Wrapper.Point StartPoint
        {
            get => (Wrapper.Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

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