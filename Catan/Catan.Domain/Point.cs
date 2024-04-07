namespace Catan.Domain;

public sealed class Point(int x, int y)
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public bool Equals(Point coordinatesToCompare)
        => X == coordinatesToCompare.X && Y == coordinatesToCompare.Y;

    public static Point Add(Point c1, Point c2)
    => new(c1.X + c2.X, c1.Y + c2.Y);

    public static Point Subtract(Point c1, Point c2)
    => new(c2.X - c1.X, c2.Y - c1.Y);
}
