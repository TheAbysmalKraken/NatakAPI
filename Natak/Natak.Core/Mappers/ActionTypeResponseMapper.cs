using Natak.Core.Models;
using Natak.Domain.Enums;

namespace Natak.Core.Mappers;

public static class ActionTypeResponseMapper
{
    private static readonly Dictionary<ActionType, ActionTypeResponse> mapping = new()
    {
        { ActionType.BuildVillage, ActionTypeResponse.BuildVillage },
        { ActionType.BuildRoad, ActionTypeResponse.BuildRoad },
        { ActionType.BuildTown, ActionTypeResponse.BuildTown },
        { ActionType.FirstSetupFinished, ActionTypeResponse.EndTurn },
        { ActionType.SecondSetupFinished, ActionTypeResponse.EndTurn },
        { ActionType.RollDice, ActionTypeResponse.RollDice },
        { ActionType.EndTurn, ActionTypeResponse.EndTurn },
        { ActionType.Trade, ActionTypeResponse.Trade },
        { ActionType.PlaySoldierCard, ActionTypeResponse.PlayGrowthCard },
        { ActionType.PlayRoamingCard, ActionTypeResponse.PlayGrowthCard },
        { ActionType.PlayWealthCard, ActionTypeResponse.PlayGrowthCard },
        { ActionType.PlayGathererCard, ActionTypeResponse.PlayGrowthCard },
        { ActionType.RollSeven, ActionTypeResponse.RollDice },
        { ActionType.AllResourcesDiscarded, ActionTypeResponse.DiscardResources },
        { ActionType.MoveThief, ActionTypeResponse.MoveThief },
        { ActionType.StealResource, ActionTypeResponse.StealResource },
        { ActionType.BuyGrowthCard, ActionTypeResponse.BuyGrowthCard }
    };

    public static ActionTypeResponse? FromDomain(ActionType actionType)
    {
        return mapping.TryGetValue(actionType, out var actionTypeResponse)
            ? actionTypeResponse
            : null;
    }
}
