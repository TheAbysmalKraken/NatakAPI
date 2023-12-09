namespace Catan.Domain;

public sealed class Coordinates
{
    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; private set; }

    public int Y { get; private set; }

    public bool Equals(Coordinates coordinatesToCompare)
        => X == coordinatesToCompare.X && Y == coordinatesToCompare.Y;

    public static Coordinates Add(Coordinates c1, Coordinates c2)
    => new(c1.X + c2.X, c1.Y + c2.Y);

    public static Coordinates Subtract(Coordinates c1, Coordinates c2)
    => new(c2.X - c1.X, c2.Y - c1.Y);
}
