using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public sealed class PointDto : IDto<Point, PointDto>
{
    public required int X { get; init; }
    
    public required int Y { get; init; }
    
    public static PointDto FromDomain(Point domain)
    {
        return new PointDto()
        {
            X = domain.X,
            Y = domain.Y
        };
    }

    public Point ToDomain()
    {
        return new Point(X, Y);
    }
}