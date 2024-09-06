using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain.Managers;

public sealed class PlayerTradeManager(int playerCount)
{
    private TradeOffer tradeOffer = TradeOffer.Inactive();
    private readonly int playerCount = playerCount;

    public readonly Dictionary<PlayerColour, List<PlayerColour>> embargoes = [];

    public Dictionary<PlayerColour, List<PlayerColour>> Embargoes => embargoes;

    public TradeOffer TradeOffer => tradeOffer;

    public void Inactive() => tradeOffer = TradeOffer.Inactive();

    public Result CreateOffer(
        Player offeringPlayer,
        Dictionary<ResourceType, int> offeredResources,
        Dictionary<ResourceType, int> requestedResources)
    {
        var hasResources = offeringPlayer.HasResourceCards(offeredResources);

        if (hasResources.IsFailure)
        {
            return hasResources;
        }

        tradeOffer = new()
        {
            IsActive = true,
            OfferingPlayer = offeringPlayer.Colour,
            Offer = offeredResources,
            Request = requestedResources
        };

        UpdateTradeRejectionsFromEmbargoes();

        return Result.Success();
    }

    public void CancelOffer()
    {
        tradeOffer = TradeOffer.Inactive();
    }

    public Result RejectOffer(PlayerColour rejectingPlayer)
    {
        if (rejectingPlayer == tradeOffer.OfferingPlayer)
        {
            return Result.Failure(PlayerErrors.CannotTradeWithSelf);
        }

        if (!tradeOffer.IsActive)
        {
            return Result.Failure(GameErrors.TradeOfferNotActive);
        }

        if (tradeOffer.RejectedBy.Contains(rejectingPlayer))
        {
            return Result.Failure(PlayerErrors.AlreadyRejectedTrade);
        }

        tradeOffer.RejectedBy.Add(rejectingPlayer);

        if (tradeOffer.RejectedBy.Count == playerCount - 1)
        {
            tradeOffer = TradeOffer.Inactive();
        }

        return Result.Success();
    }

    public Result AcceptOffer(Player offeringPlayer, Player acceptingPlayer)
    {
        if (!tradeOffer.IsActive)
        {
            return Result.Failure(GameErrors.TradeOfferNotActive);
        }

        if (offeringPlayer.Colour != tradeOffer.OfferingPlayer)
        {
            throw new InvalidOperationException("Offering player is not the same as the trade offer's offering player.");
        }

        if (acceptingPlayer.Colour == tradeOffer.OfferingPlayer)
        {
            return Result.Failure(PlayerErrors.CannotTradeWithSelf);
        }

        if (tradeOffer.RejectedBy.Contains(acceptingPlayer.Colour))
        {
            return Result.Failure(PlayerErrors.AlreadyRejectedTrade);
        }

        var hasResources = acceptingPlayer.HasResourceCards(tradeOffer.Request);

        if (hasResources.IsFailure)
        {
            return hasResources;
        }

        acceptingPlayer.RemoveResourceCards(tradeOffer.Request);
        acceptingPlayer.AddResourceCards(tradeOffer.Offer);
        offeringPlayer.RemoveResourceCards(tradeOffer.Offer);
        offeringPlayer.AddResourceCards(tradeOffer.Request);

        tradeOffer = TradeOffer.Inactive();

        return Result.Success();
    }

    public Result AddEmbargo(PlayerColour embargoingPlayer, PlayerColour embargoedPlayer)
    {
        if (embargoingPlayer == embargoedPlayer)
        {
            return Result.Failure(PlayerErrors.CannotEmbargoSelf);
        }

        if (!embargoes.ContainsKey(embargoingPlayer))
        {
            embargoes[embargoingPlayer] = [];
        }

        if (embargoes[embargoingPlayer].Contains(embargoedPlayer))
        {
            return Result.Failure(PlayerErrors.AlreadyEmbargoed);
        }

        embargoes[embargoingPlayer].Add(embargoedPlayer);

        return Result.Success();
    }

    public Result RemoveEmbargo(PlayerColour embargoingPlayer, PlayerColour embargoedPlayer)
    {
        if (!embargoes.ContainsKey(embargoingPlayer)
            || embargoes[embargoingPlayer].Count == 0)
        {
            return Result.Failure(PlayerErrors.NotEmbargoed);
        }

        embargoes[embargoingPlayer].Remove(embargoedPlayer);

        return Result.Success();
    }

    private void UpdateTradeRejectionsFromEmbargoes()
    {
        if (!tradeOffer.IsActive || tradeOffer.OfferingPlayer is null)
        {
            return;
        }

        foreach (var playerColour in embargoes.Keys)
        {
            if (playerColour == tradeOffer.OfferingPlayer)
            {
                continue;
            }

            if (embargoes[playerColour].Contains(tradeOffer.OfferingPlayer.Value))
            {
                if (!tradeOffer.RejectedBy.Contains(playerColour))
                {
                    tradeOffer.RejectedBy.Add(playerColour);
                }
            }
        }

        if (!embargoes.TryGetValue(tradeOffer.OfferingPlayer.Value, out var embargoedPlayers))
        {
            return;
        }

        foreach (var playerColour in embargoedPlayers)
        {
            if (!tradeOffer.RejectedBy.Contains(playerColour))
            {
                tradeOffer.RejectedBy.Add(playerColour);
            }
        }
    }
}
