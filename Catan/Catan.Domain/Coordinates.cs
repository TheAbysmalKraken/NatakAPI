namespace Catan.Domain
{
    public sealed class Coordinates
    {
        private int x { get; set; }

        private int y { get; set; }

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public static Coordinates Add(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c1.x + c2.x, c1.y + c2.y);
        }

        public static Coordinates Subtract(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c2.x - c1.x, c2.y - c1.y);
        }
    }
}
