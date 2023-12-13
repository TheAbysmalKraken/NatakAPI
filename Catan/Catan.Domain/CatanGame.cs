using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanGame
{
    private readonly List<CatanPlayer> players = new();
    private readonly List<int> rolledDice = new();
    private readonly Dictionary<CatanResourceType, int> remainingResourceCards = new();
    private readonly Dictionary<CatanDevelopmentCardType, int> remainingDevelopmentCards = new();
    private int knightsRequiredForLargestArmy;
    private int roadsRequiredForLongestRoad;

    public CatanGame(int numberOfPlayers)
    {
        Board = new CatanBoard();

        RollDice();
        InitialisePlayers(numberOfPlayers);
        InitialiseResourceCards();
        InitialiseDevelopmentCards();

        knightsRequiredForLargestArmy = 3;
        roadsRequiredForLongestRoad = 5;
    }

    public CatanBoard Board { get; private set; } = new();

    public CatanPlayer? LongestRoadPlayer { get; set; } = null;

    public CatanPlayer? LargestArmyPlayer { get; set; } = null;

    public int DiceTotal => rolledDice.Sum();

    public List<CatanPlayer> GetPlayers() => players;

    public List<int> GetRolledDice() => rolledDice;

    public Dictionary<CatanResourceType, int> GetRemainingResourceCards() => remainingResourceCards;

    public Dictionary<CatanDevelopmentCardType, int> GetRemainingDevelopmentCards() => remainingDevelopmentCards;

    public void NextPlayer()
    {
        UpdateLargestArmyPlayer();

        var currentPlayer = players.First();

        currentPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        players.Remove(currentPlayer);
        players.Add(currentPlayer);
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
        remainingDevelopmentCards.Clear();

        var totals = DomainConstants.GetDevelopmentCardTypeTotals();

        foreach (var type in totals.Keys)
        {
            remainingDevelopmentCards.Add(type, totals[type]);
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

    private void RollDice()
    {
        rolledDice.Clear();
        rolledDice.AddRange(DiceRoller.RollDice(2, 6));
    }

    private void UpdateLargestArmyPlayer()
    {
        var player = players.OrderByDescending(p => p.KnightsPlayed).FirstOrDefault();

        if (player != null && LargestArmyPlayer != player && player.KnightsPlayed >= knightsRequiredForLargestArmy)
        {
            LargestArmyPlayer = player;
            knightsRequiredForLargestArmy = player.KnightsPlayed + 1;
        }
    }

    private void PlayDevelopmentCard(CatanPlayer player, CatanDevelopmentCardType type)
    {
        player.PlayDevelopmentCard(type);
        remainingDevelopmentCards[type]++;
    }

    private void PlayKnightCard(CatanPlayer player)
    {
        player.KnightsPlayed++;
        PlayDevelopmentCard(player, CatanDevelopmentCardType.Knight);
    }
}
