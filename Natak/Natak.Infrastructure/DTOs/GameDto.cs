using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public sealed class GameDto : IDto<Game, GameDto>
{
    public required Stack<DiceRoll> DiceRolls { get; init; }
    
    public required int RoamingRoadsLeftToPlace { get; init; }
    
    public required string Id { get; init; }
    
    public required BoardDto Board { get; init; }
    
    public required StateManagerDto StateManager { get; init; }
    
    public required PlayerTradeManagerDto PlayerTradeManager { get; init; }
    
    public required BankTradeManagerDto BankTradeManager { get; init; }
    
    public required PlayerManagerDto PlayerManager { get; init; }
    
    public required bool GrowthCardPlayed { get; init; }
    
    public static GameDto FromDomain(Game domain)
    {
        return new GameDto()
        {
            DiceRolls = domain.GetDiceRolls(),
            RoamingRoadsLeftToPlace = domain.GetRoamingRoadsLeftToPlace(),
            Id = domain.Id,
            Board = BoardDto.FromDomain(domain.Board),
            StateManager = StateManagerDto.FromDomain(domain.StateManager),
            PlayerTradeManager = PlayerTradeManagerDto.FromDomain(domain.TradeManager),
            BankTradeManager = BankTradeManagerDto.FromDomain(domain.BankManager),
            PlayerManager = PlayerManagerDto.FromDomain(domain.PlayerManager),
            GrowthCardPlayed = domain.GrowthCardPlayed
        };
    }

    public Game ToDomain()
    {
        return new Game(
            Id,
            Board.ToDomain(),
            StateManager.ToDomain(),
            PlayerTradeManager.ToDomain(),
            BankTradeManager.ToDomain(),
            PlayerManager.ToDomain(),
            GrowthCardPlayed,
            DiceRolls,
            RoamingRoadsLeftToPlace);
    }
}