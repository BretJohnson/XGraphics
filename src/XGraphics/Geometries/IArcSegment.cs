namespace XGraphics.Geometries
{
    [GraphicsModelObject]
    public interface IArcSegment : IPathSegment
    {
        Point Point { get; }

        Size Size { get; }

        [ModelDefaultValue(0.0)]
        double RotationAngle { get; }

        [ModelDefaultValue(false)]
        bool IsLargeArc { get; }

        [ModelDefaultValue(SweepDirection.Counterclockwise)]
        SweepDirection SweepDirection { get; }
    }
}
