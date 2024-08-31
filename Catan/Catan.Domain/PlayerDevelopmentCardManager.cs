using Catan.Domain.Enums;

namespace Catan.Domain;

public sealed class PlayerDevelopmentCardManager : CardManager<DevelopmentCardType>
{
    private readonly Dictionary<DevelopmentCardType, int> onHoldCards = [];

    public Dictionary<DevelopmentCardType, int> OnHoldCards => onHoldCards;

    public void Add(DevelopmentCardType card)
    {
        if (card == DevelopmentCardType.VictoryPoint)
        {
            base.Add(card);

            return;
        }

        if (onHoldCards.TryGetValue(card, out int value))
        {
            onHoldCards[card] = value + 1;
        }
        else
        {
            onHoldCards[card] = 1;
        }
    }

    public void CycleOnHoldCards()
    {
        foreach (var cardType in onHoldCards.Keys)
        {
            var count = onHoldCards[cardType];

            Add(cardType, count);
        }

        onHoldCards.Clear();
    }
}
