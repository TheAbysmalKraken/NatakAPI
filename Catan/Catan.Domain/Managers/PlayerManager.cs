using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain.Managers;

public sealed class PlayerManager
{
    private const int MINIMUM_KNIGHTS_FOR_LARGEST_ARMY = 3;

    private static readonly Random random = new();
    private readonly Dictionary<PlayerColour, Player> players = [];
    private readonly List<PlayerColour> playerOrder = [];
    private readonly List<PlayerColour> setupPlayerOrder = [];
    private int currentPlayerIndex = 0;
    private int largestArmySize = 0;
    private PlayerColour largestArmyPlayerColour = PlayerColour.None;

    public PlayerManager(int playerCount)
    {
        AddPlayers(playerCount);
    }

    public Player CurrentPlayer => players[CurrentPlayerColour];

    public PlayerColour CurrentPlayerColour => IsSetup
        ? setupPlayerOrder[currentPlayerIndex]
        : playerOrder[currentPlayerIndex];

    public List<Player> Players => [.. players.Values];

    public bool IsSetup { get; set; } = true;

    public bool IsSecondRoundOfSetup => currentPlayerIndex >= playerOrder.Count;

    public bool PlayersNeedToDiscard => DoPlayersNeedToDiscard();

    public Player? WinningPlayer => GetWinningPlayer();

    public Player? LargestArmyPlayer => GetLargestArmyPlayer();

    public Player? LongestRoadPlayer => GetLongestRoadPlayer();

    public Player? GetPlayer(PlayerColour playerColour) => players.TryGetValue(playerColour, out var player) ? player : null;

    public void NextPlayer()
    {
        if (IsSetup)
        {
            NextSetupPlayer();
        }
        else
        {
            NextMainPlayer();
        }

        foreach (var player in players.Values)
        {
            player.CycleDevelopmentCards();
        }
    }

    public void GivePlayerMonopolyResource(Player player, ResourceType resourceType)
    {
        foreach (var otherPlayer in players.Values)
        {
            if (otherPlayer.Colour != player.Colour)
            {
                var cardCount = otherPlayer.CountResourceCard(resourceType);

                otherPlayer.RemoveResourceCard(resourceType, cardCount);
                player.AddResourceCard(resourceType, cardCount);
            }
        }
    }

    public Result StealFromPlayer(PlayerColour thiefColour, PlayerColour victimColour)
    {
        var thief = players[thiefColour];
        var victim = players[victimColour];

        if (thief is null || victim is null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        if (thief.Colour == victim.Colour)
        {
            return Result.Failure(PlayerErrors.CannotStealFromSelf);
        }

        var stolenCardType = victim.RemoveRandomResourceCard();

        if (stolenCardType is null)
        {
            return Result.Success();
        }

        thief.AddResourceCard(stolenCardType.Value, 1);

        return Result.Success();
    }

    public void GivePort(PlayerColour playerColour, PortType portType)
    {
        if (!players.TryGetValue(playerColour, out var player))
        {
            return;
        }

        player.Ports.Add(portType);
    }

    public void UpdateLongestRoadPlayer(PlayerColour playerColour)
    {
        if (!players.TryGetValue(playerColour, out var player))
        {
            return;
        }

        if (player.ScoreManager.HasLongestRoad)
        {
            return;
        }

        var currentLongestRoadPlayer = Players.FirstOrDefault(p => p.ScoreManager.HasLongestRoad);

        if (currentLongestRoadPlayer != null)
        {
            currentLongestRoadPlayer.RemoveLongestRoadCard();
        }

        player.AddLongestRoadCard();
    }

    public void UpdateLargestArmyPlayer()
    {
        var newLargestArmyPlayer = players.Values
            .FirstOrDefault(p => p.KnightsPlayed > largestArmySize);

        if (newLargestArmyPlayer is null)
        {
            return;
        }

        if (newLargestArmyPlayer.KnightsPlayed < MINIMUM_KNIGHTS_FOR_LARGEST_ARMY)
        {
            return;
        }

        if (largestArmyPlayerColour != PlayerColour.None)
        {
            players[largestArmyPlayerColour].RemoveLargestArmyCard();
        }

        newLargestArmyPlayer.AddLargestArmyCard();

        largestArmySize = newLargestArmyPlayer.KnightsPlayed;
        largestArmyPlayerColour = newLargestArmyPlayer.Colour;
    }

    public void CalculateDiscardRequirements()
    {
        foreach (var player in players.Values)
        {
            var resourceCount = player.ResourceCardManager.CountAll();

            if (resourceCount > 7)
            {
                player.CardsToDiscard = resourceCount / 2;
            }
        }
    }

    private void AddPlayers(int playerCount)
    {
        players.Clear();
        playerOrder.Clear();

        var playerColours = Enum.GetValues<PlayerColour>().ToList();
        playerColours.Remove(PlayerColour.None);
        playerColours = playerColours.Take(playerCount).ToList();

        foreach (var playerColour in playerColours)
        {
            players[playerColour] = new(playerColour);
            playerOrder.Add(playerColour);
        }

        ShufflePlayers();

        var tempOrder = playerOrder.ToList();
        tempOrder.Reverse();

        setupPlayerOrder.AddRange(playerOrder);
        setupPlayerOrder.AddRange(tempOrder);
    }

    private void ShufflePlayers()
    {
        var totalPlayers = players.Keys.Count;

        while (totalPlayers > 1)
        {
            var playerIndexToSwapWith = random.Next(totalPlayers);
            totalPlayers--;
            (playerOrder[totalPlayers], playerOrder[playerIndexToSwapWith])
                = (playerOrder[playerIndexToSwapWith], playerOrder[totalPlayers]);
        }
    }

    private void NextSetupPlayer()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= setupPlayerOrder.Count)
        {
            IsSetup = false;
            currentPlayerIndex = 0;
        }
    }

    private void NextMainPlayer()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= playerOrder.Count)
        {
            currentPlayerIndex = 0;
        }
    }

    private Player? GetWinningPlayer()
    {
        return players.Values
            .Where(p => p.ScoreManager.TotalPoints >= 10)
            .FirstOrDefault();
    }

    private Player? GetLargestArmyPlayer()
    {
        return players.Values
            .FirstOrDefault(p => p.ScoreManager.HasLargestArmy);
    }

    private Player? GetLongestRoadPlayer()
    {
        return players.Values
            .FirstOrDefault(p => p.ScoreManager.HasLongestRoad);
    }

    private bool DoPlayersNeedToDiscard()
    {
        return players.Values
            .Any(p => p.CardsToDiscard > 0);
    }
}
