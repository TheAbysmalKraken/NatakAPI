using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class TileManager
{
    private static readonly Random random = new();
    
    private readonly int size;
    private readonly List<Tile> tiles = [];

    public TileManager(List<Tile> tiles)
    {
        this.tiles = tiles;
    }
    
    public TileManager(int size)
    {
        this.size = size;
        
        GenerateTiles();
    }
    
    public List<Tile> GetTiles()
        => tiles;
    
    public List<Tile> GetTile(ResourceType type)
        => tiles.Where(x => x.Type == type).ToList();
    
    public List<Tile> GetTile(int activationNumber)
        => tiles.Where(x => x.ActivationNumber == activationNumber).ToList();
    
    public Tile? GetTile(Point point)
        => tiles.FirstOrDefault(x => x.Point.Equals(point));
    
    private void GenerateTiles()
    {
        var remainingResourceTileTypes = DomainConstants.GetTileResourceTypeTotals();
        var remainingActivationNumbers = DomainConstants.GetTileActivationNumberTotals();

        for (var x = 0; x < size; x++)
        {
            for (var y = 0; y < size; y++)
            {
                if (x + y < 2 || x + y > size + 1)
                {
                    continue;
                }
                
                var point = new Point(x, y);
                
                AddNewTile(
                    point,
                    remainingResourceTileTypes,
                    remainingActivationNumbers);
            }
        }
    }
    
    private void AddNewTile(
        Point point,
        Dictionary<ResourceType, int> remainingResourceTileTypes,
        Dictionary<int, int> remainingActivationNumbers)
    {
        if (remainingResourceTileTypes is null || remainingResourceTileTypes.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingResourceTileTypes)} must not be null or empty.");
        }

        if (remainingActivationNumbers is null || remainingActivationNumbers.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingActivationNumbers)} must not be null or empty.");
        }

        ResourceType type;
        var lowestTileTypeNum = (int)remainingResourceTileTypes.First().Key;
        var highestTileTypeNum = (int)remainingResourceTileTypes.Last().Key;

        do
        {
            type = (ResourceType)random.Next(lowestTileTypeNum, highestTileTypeNum + 1);
        }
        while (remainingResourceTileTypes[type] <= 0);

        remainingResourceTileTypes[type]--;

        if (type == ResourceType.None)
        {
            tiles.Add(new Tile(type, 0, point));
            return;
        }

        int activationNumber;
        var lowestActivationNum = remainingActivationNumbers.First().Key;
        var highestActivationNum = remainingActivationNumbers.Last().Key;

        do
        {
            activationNumber = random.Next(lowestActivationNum, highestActivationNum + 1);
        }
        while (remainingActivationNumbers[activationNumber] <= 0);

        remainingActivationNumbers[activationNumber]--;

        tiles.Add(new Tile(type, activationNumber, point));
    }
}