namespace XGraphics
{
    public struct Point
    {
        public static readonly Point Default = new Point(0, 0);
        public static readonly Point CenterDefault = new Point(0.5, 0.5);


        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public Point WithX(double x) => new Point(x, Y);

        public Point WithY(double y) => new Point(X, y);
    }
}
