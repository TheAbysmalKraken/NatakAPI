using Catan.Domain.Errors;

namespace Catan.Domain;

public class CardManager<TCard> where TCard : Enum
{
    protected readonly Dictionary<TCard, int> cards = [];

    public Dictionary<TCard, int> Cards => cards;

    public bool Has(TCard card)
    {
        return Count(card) > 0;
    }

    public int Count(TCard card)
    {
        return cards.TryGetValue(card, out int value) ? value : 0;
    }

    public int CountAll()
    {
        return cards.Values.Sum();
    }

    public void Add(TCard card, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0, nameof(count));

        if (cards.TryGetValue(card, out int value))
        {
            cards[card] = value + count;
        }
        else
        {
            cards[card] = count;
        }
    }

    public void Set(TCard card, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0, nameof(count));

        cards[card] = count;
    }

    public Result Remove(TCard card, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0, nameof(count));

        if (cards.TryGetValue(card, out int value))
        {
            if (value >= count)
            {
                cards[card] = value - count;
                return Result.Success();
            }
            else
            {
                return Result.Failure(CardManagerErrors.NotEnough);
            }
        }
        else
        {
            return Result.Failure(CardManagerErrors.NotFound);
        }
    }
}
