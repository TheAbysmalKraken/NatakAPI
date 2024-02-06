namespace Catan.Domain;

public sealed class Coordinates(int x, int y)
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public bool Equals(Coordinates coordinatesToCompare)
        => X == coordinatesToCompare.X && Y == coordinatesToCompare.Y;

    public static Coordinates Add(Coordinates c1, Coordinates c2)
    => new(c1.X + c2.X, c1.Y + c2.Y);

    public static Coordinates Subtract(Coordinates c1, Coordinates c2)
    => new(c2.X - c1.X, c2.Y - c1.Y);
}
