using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanGame
{
    private readonly List<CatanPlayer> players = new();
    private readonly List<int> rolledDice = new();
    private readonly Dictionary<CatanResourceType, int> remainingResourceCards = new();
    private readonly Dictionary<CatanDevelopmentCardType, int> remainingDevelopmentCardTotals = new();
    private readonly List<CatanDevelopmentCardType> remainingDevelopmentCards = new();
    private int knightsRequiredForLargestArmy;
    private int roadsRequiredForLongestRoad;
    private bool developmentCardPlayedThisTurn;

    public CatanGame(int numberOfPlayers)
    {
        Board = new CatanBoard();

        RollDice();
        InitialisePlayers(numberOfPlayers);
        InitialiseResourceCards();
        InitialiseDevelopmentCards();

        knightsRequiredForLargestArmy = 3;
        roadsRequiredForLongestRoad = 5;
        developmentCardPlayedThisTurn = false;
    }

    public CatanBoard Board { get; private set; } = new();

    public CatanPlayer? LongestRoadPlayer { get; set; } = null;

    public CatanPlayer? LargestArmyPlayer => players.FirstOrDefault(p => p.HasLargestArmy);

    public int DiceTotal => rolledDice.Sum();

    public CatanPlayer CurrentPlayer => players.First();

    public List<CatanPlayer> GetPlayers() => players;

    public List<int> GetRolledDice() => rolledDice;

    public Dictionary<CatanResourceType, int> GetRemainingResourceCards()
        => remainingResourceCards;

    public Dictionary<CatanDevelopmentCardType, int> GetRemainingDevelopmentCardTotals
        => remainingDevelopmentCardTotals;

    public List<CatanDevelopmentCardType> GetRemainingDevelopmentCards()
        => remainingDevelopmentCards;

    public void NextPlayer()
    {
        UpdateLargestArmyPlayer();

        developmentCardPlayedThisTurn = false;

        var currentPlayer = CurrentPlayer;

        currentPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        players.Remove(currentPlayer);
        players.Add(currentPlayer);
    }

    public void RollDice()
    {
        rolledDice.Clear();
        rolledDice.AddRange(DiceRoller.RollDice(2, 6));
    }

    public void PlayDevelopmentCard(CatanPlayer player, CatanDevelopmentCardType type)
    {
        if (developmentCardPlayedThisTurn || !player.CanPlayDevelopmentCardOfType(type))
        {
            return;
        }

        developmentCardPlayedThisTurn = true;

        switch (type)
        {
            case CatanDevelopmentCardType.Knight:
                PlayKnightCard(player);
                break;

            default:
                throw new ArgumentException($"Invalid development card type: '{type}'");
        }

        player.PlayDevelopmentCard(type);

        remainingDevelopmentCardTotals[type]++;
        remainingDevelopmentCards.Add(type);
    }

    private void InitialisePlayers(int numberOfPlayers)
    {
        players.Clear();

        for (var i = 0; i < numberOfPlayers; i++)
        {
            CatanPlayerColour playerColour = (CatanPlayerColour)(i + 1);
            var newPlayer = new CatanPlayer(playerColour);

            players.Add(newPlayer);
        }
    }

    private void InitialiseDevelopmentCards()
    {
        remainingDevelopmentCardTotals.Clear();

        var totals = DomainConstants.GetDevelopmentCardTypeTotals();

        foreach (var type in totals.Keys)
        {
            remainingDevelopmentCardTotals.Add(type, totals[type]);
            for (var i = 0; i < totals[type]; i++)
            {
                remainingDevelopmentCards.Add(type);
            }
        }

        ShuffleDevelopmentCards();
    }

    private void ShuffleDevelopmentCards()
    {
        var random = new Random();
        var totalDevelopmentCards = remainingDevelopmentCards.Count;

        while (totalDevelopmentCards > 1)
        {
            var cardIndexToSwapWith = random.Next(totalDevelopmentCards);
            totalDevelopmentCards--;
            (remainingDevelopmentCards[totalDevelopmentCards], remainingDevelopmentCards[cardIndexToSwapWith]) = (remainingDevelopmentCards[cardIndexToSwapWith], remainingDevelopmentCards[totalDevelopmentCards]);
        }
    }

    private void InitialiseResourceCards()
    {
        remainingResourceCards.Clear();

        var totals = DomainConstants.GetBankResourceTotals();

        foreach (var type in totals.Keys)
        {
            remainingResourceCards.Add(type, totals[type]);
        }
    }

    private void UpdateLargestArmyPlayer()
    {
        var player = players.OrderByDescending(p => p.KnightsPlayed).FirstOrDefault();

        if (player != null && LargestArmyPlayer != player && player.KnightsPlayed >= knightsRequiredForLargestArmy)
        {
            if (LargestArmyPlayer != null)
            {
                LargestArmyPlayer.HasLargestArmy = false;
            }

            player.HasLargestArmy = true;
            knightsRequiredForLargestArmy = player.KnightsPlayed + 1;
        }
    }

    private void PlayKnightCard(CatanPlayer player)
    {
        player.KnightsPlayed++;
    }
}
