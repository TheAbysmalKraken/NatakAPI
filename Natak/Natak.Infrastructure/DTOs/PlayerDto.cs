using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerDto : IDto<Player, PlayerDto>
{
    public required PlayerColour Colour { get; init; }
    
    public required PlayerResourceCardManagerDto ResourceCardManager { get; init; }
    
    public required PlayerGrowthCardManagerDto GrowthCardManager { get; init; }
    
    public required PlayerPieceManagerDto PieceManager { get; init; }
    
    public required PlayerScoreManagerDto ScoreManager { get; init; }
    
    public required int SoldiersPlayed { get; init; }
    
    public required List<PortType> Ports { get; init; }
    
    public required int CardsToDiscard { get; init; }
    
    public static PlayerDto FromDomain(Player domain)
    {
        return new PlayerDto()
        {
            Colour = domain.Colour,
            ResourceCardManager = PlayerResourceCardManagerDto.FromDomain(domain.ResourceCardManager),
            GrowthCardManager = PlayerGrowthCardManagerDto.FromDomain(domain.GrowthCardManager),
            PieceManager = PlayerPieceManagerDto.FromDomain(domain.PieceManager),
            ScoreManager = PlayerScoreManagerDto.FromDomain(domain.ScoreManager),
            SoldiersPlayed = domain.SoldiersPlayed,
            Ports = domain.Ports,
            CardsToDiscard = domain.CardsToDiscard
        };
    }

    public Player ToDomain()
    {
        return new Player(
            Colour,
            ResourceCardManager.ToDomain(),
            GrowthCardManager.ToDomain(),
            PieceManager.ToDomain(),
            ScoreManager.ToDomain(),
            SoldiersPlayed,
            Ports,
            CardsToDiscard);
    }
}