using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerScoreManagerDto :
    IDto<PlayerScoreManager, PlayerScoreManagerDto>
{
    public required int VisiblePoints { get; init; }
    
    public required int HiddenPoints { get; init; }
    
    public required bool HasLargestArmy { get; init; }
    
    public required bool HasLongestRoad { get; init; }
    
    public static PlayerScoreManagerDto FromDomain(PlayerScoreManager domain)
    {
        return new PlayerScoreManagerDto()
        {
            VisiblePoints = domain.VisiblePoints,
            HiddenPoints = domain.HiddenPoints,
            HasLargestArmy = domain.HasLargestArmy,
            HasLongestRoad = domain.HasLongestRoad
        };
    }

    public PlayerScoreManager ToDomain()
    {
        return new PlayerScoreManager(
            VisiblePoints,
            HiddenPoints,
            HasLargestArmy,
            HasLongestRoad);
    }
}