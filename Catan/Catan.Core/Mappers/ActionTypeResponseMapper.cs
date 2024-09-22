using Catan.Core.Models;
using Catan.Domain.Enums;

namespace Catan.Core.Mappers;

public static class ActionTypeResponseMapper
{
    private static readonly Dictionary<ActionType, ActionTypeResponse> mapping = new()
    {
        { ActionType.BuildSettlement, ActionTypeResponse.BuildSettlement },
        { ActionType.BuildRoad, ActionTypeResponse.BuildRoad },
        { ActionType.BuildCity, ActionTypeResponse.BuildCity },
        { ActionType.FirstSetupFinished, ActionTypeResponse.EndTurn },
        { ActionType.SecondSetupFinished, ActionTypeResponse.EndTurn },
        { ActionType.RollDice, ActionTypeResponse.RollDice },
        { ActionType.EndTurn, ActionTypeResponse.EndTurn },
        { ActionType.Trade, ActionTypeResponse.Trade },
        { ActionType.PlayKnightCard, ActionTypeResponse.PlayDevelopmentCard },
        { ActionType.PlayRoadBuildingCard, ActionTypeResponse.PlayDevelopmentCard },
        { ActionType.PlayYearOfPlentyCard, ActionTypeResponse.PlayDevelopmentCard },
        { ActionType.PlayMonopolyCard, ActionTypeResponse.PlayDevelopmentCard },
        { ActionType.RollSeven, ActionTypeResponse.RollDice },
        { ActionType.AllResourcesDiscarded, ActionTypeResponse.DiscardResources },
        { ActionType.MoveRobber, ActionTypeResponse.MoveRobber },
        { ActionType.StealResource, ActionTypeResponse.StealResource }
    };

    public static ActionTypeResponse? FromDomain(ActionType actionType)
    {
        return mapping.TryGetValue(actionType, out var actionTypeResponse)
            ? actionTypeResponse
            : null;
    }
}
