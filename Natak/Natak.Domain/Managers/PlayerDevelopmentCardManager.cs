using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerDevelopmentCardManager : ItemManager<DevelopmentCardType>
{
    private readonly Dictionary<DevelopmentCardType, int> onHoldCards = [];

    public Dictionary<DevelopmentCardType, int> Cards => items;

    public Dictionary<DevelopmentCardType, int> OnHoldCards => onHoldCards;

    public bool HasOnHold(DevelopmentCardType card)
    {
        return CountOnHold(card) > 0;
    }

    public int CountOnHold(DevelopmentCardType card)
    {
        return onHoldCards.TryGetValue(card, out int value) ? value : 0;
    }

    public int CountAllOnHold()
    {
        return onHoldCards.Values.Sum();
    }

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
