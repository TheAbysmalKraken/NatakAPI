namespace Catan.Domain
{
    public sealed class Coordinates
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Coordinates coordinatesToCompare)
        {
            return (this.X == coordinatesToCompare.X && this.Y == coordinatesToCompare.Y);
        }

        public static Coordinates Add(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static Coordinates Subtract(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c2.X - c1.X, c2.Y - c1.Y);
        }
    }
}
