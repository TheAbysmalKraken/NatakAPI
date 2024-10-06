using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerManagerDto :
    IDto<PlayerManager, PlayerManagerDto>
{
    public required Dictionary<PlayerColour, PlayerDto> Players { get; init; }
    
    public required List<PlayerColour> PlayerOrder { get; init; }
    
    public required List<PlayerColour> SetupPlayerOrder { get; init; }
    
    public required int CurrentPlayerIndex { get; init; }
    
    public required bool IsSetup { get; init; }
    
    public static PlayerManagerDto FromDomain(PlayerManager domain)
    {
        return new PlayerManagerDto()
        {
            Players = domain.GetPlayersDictionary()
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => PlayerDto.FromDomain(kvp.Value)),
            PlayerOrder = domain.GetPlayerOrder(),
            SetupPlayerOrder = domain.GetSetupPlayerOrder(),
            CurrentPlayerIndex = domain.GetCurrentPlayerIndex(),
            IsSetup = domain.IsSetup
        };
    }

    public PlayerManager ToDomain()
    {
        return new PlayerManager(
            Players.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToDomain()),
            PlayerOrder,
            SetupPlayerOrder,
            CurrentPlayerIndex,
            IsSetup);
    }
}