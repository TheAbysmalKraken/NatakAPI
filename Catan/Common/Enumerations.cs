namespace Catan.Common
{
    public static class Enumerations
    {
        public enum CatanResourceType
        {
            Unknown = 0,
            Wood = 1,
            Brick = 2,
            Sheep = 3,
            Wheat = 4,
            Ore = 5,
            Desert = 6
        }

        public enum CatanPlayerColour
        {
            None = 0,
            Red = 1,
            Blue = 2,
            Green = 3,
            Yellow = 4
        }

        public enum CatanBuildingType
        {
            None = 0,
            Road = 1,
            Settlement = 2,
            City = 3
        }

        public enum CatanPortType
        {
            None = 0,
            Wood = 1,
            Brick = 2,
            Sheep = 3,
            Wheat = 4,
            Ore = 5,
            ThreeToOne = 6
        }
    }
}