namespace XGraphics
{
    public struct Point
    {
        public static readonly Point Default = new Point(0, 0);


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
