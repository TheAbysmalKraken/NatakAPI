using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerGrowthCardManager : ItemManager<GrowthCardType>
{
    private readonly Dictionary<GrowthCardType, int> onHoldCards = [];

    public Dictionary<GrowthCardType, int> Cards => items;

    public Dictionary<GrowthCardType, int> OnHoldCards => onHoldCards;

    public bool HasOnHold(GrowthCardType card)
    {
        return CountOnHold(card) > 0;
    }

    public int CountOnHold(GrowthCardType card)
    {
        return onHoldCards.TryGetValue(card, out int value) ? value : 0;
    }

    public int CountAllOnHold()
    {
        return onHoldCards.Values.Sum();
    }

    public void Add(GrowthCardType card)
    {
        if (card == GrowthCardType.VictoryPoint)
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
