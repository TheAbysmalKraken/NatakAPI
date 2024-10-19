using Natak.Domain.Enums;

namespace Natak.Domain;

public class Road(
    PlayerColour colour,
    Point firstPoint,
    Point secondPoint)
{
    public Road(Point firstPoint, Point secondPoint)
        : this(PlayerColour.None, firstPoint, secondPoint)
    {
    }
    
    public PlayerColour Colour { get; private set; } = colour;
    
    public Point FirstPoint { get; } = firstPoint;

    public Point SecondPoint { get; } = secondPoint;
    
    public void SetColour(PlayerColour colour)
    {
        Colour = colour;
    }
    
    public bool IsAtPoints(Point firstPoint, Point secondPoint)
        => (FirstPoint.Equals(firstPoint) && SecondPoint.Equals(secondPoint)) ||
           (FirstPoint.Equals(secondPoint) && SecondPoint.Equals(firstPoint));
}
