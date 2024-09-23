using Natak.Domain.Enums;

namespace Natak.Domain.Factories;

internal static class GameFactory
{
    public static Game Create(GameFactoryOptions? options = null)
    {
        options ??= new GameFactoryOptions();

        var game = new Game(options.PlayerCount);

        if (options.IsEndOfSetup)
        {
            game.GetToEndOfSetup(options.PlayerCount);
        }
        else if (!options.IsSetup)
        {
            game.GetOutOfSetup(options.PlayerCount);
        }

        foreach (var player in game.PlayerManager.Players)
        {
            if (options.GivePlayersResources)
            {
                player.GiveManyResources();
            }

            if (options.GivePlayersDevelopmentCards)
            {
                player.GiveDevelopmentCards();
            }

            if (options.RemovePlayersPieces)
            {
                player.RemoveAllPieces();
            }

            if (options.PlayersVisiblePoints != null)
            {
                player.SetVisiblePoints(options.PlayersVisiblePoints.Value);
            }

            if (options.PlayersHiddenPoints != null)
            {
                player.SetHiddenPoints(options.PlayersHiddenPoints.Value);
            }
        }

        if (options.HasRolledSeven)
        {
            game.HasRolledSeven();
        }
        else if (options.HasRolled)
        {
            game.HasRolledDice();
        }

        if (options.PrepareLongestRoad)
        {
            game.Board.PrepareLongestRoad(game.PlayerManager.CurrentPlayerColour);
        }

        if (options.PrepareSettlementPlacement)
        {
            game.Board.PrepareSettlementPlacement(game.PlayerManager.CurrentPlayerColour);
        }

        if (options.PrepareLargestArmy)
        {
            game.PrepareLargestArmy(game.PlayerManager.CurrentPlayerColour);
        }

        return game;
    }

    private static void GetToEndOfSetup(this Game game, int playerCount)
    {
        for (var i = 0; i < playerCount; i++)
        {
            var x = 2 + 2 * i;
            game.PlaceSettlement(new(x, 3));
            game.PlaceRoad(new(x, 3), new(x, 2));
            game.EndTurn();
        }

        for (var i = 0; i < playerCount; i++)
        {
            var x = 3 + 2 * i;
            game.PlaceSettlement(new(x, 1));
            game.PlaceRoad(new(x, 1), new(x, 2));

            if (i != playerCount - 1)
            {
                game.EndTurn();
            }
        }
    }

    private static void GetOutOfSetup(this Game game, int playerCount)
    {
        GetToEndOfSetup(game, playerCount);
        game.EndTurn();
    }

    private static void GiveManyResources(this Player player)
    {
        do
        {
            player.RemoveRandomResourceCard();
        }
        while (player.ResourceCardManager.CountAll() > 0);

        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 },
            { ResourceType.Brick, 10 },
            { ResourceType.Sheep, 10 },
            { ResourceType.Wheat, 10 },
            { ResourceType.Ore, 10 }
        };

        player.AddResourceCards(resources);
    }

    private static void GiveDevelopmentCards(this Player player)
    {
        player.AddDevelopmentCard(DevelopmentCardType.Knight);
        player.AddDevelopmentCard(DevelopmentCardType.RoadBuilding);
        player.AddDevelopmentCard(DevelopmentCardType.YearOfPlenty);
        player.AddDevelopmentCard(DevelopmentCardType.Monopoly);
        player.CycleDevelopmentCards();
    }

    private static void RemoveAllPieces(this Player player)
    {
        bool piecesLeftToRemove;

        do
        {
            player.RemovePiece(BuildingType.Road);
            player.RemovePiece(BuildingType.Settlement);
            player.RemovePiece(BuildingType.City);

            var existingRoads = player.PieceManager.Roads;
            var existingSettlements = player.PieceManager.Settlements;
            var existingCities = player.PieceManager.Cities;

            piecesLeftToRemove = existingRoads + existingSettlements + existingCities > 0;
        }
        while (piecesLeftToRemove);
    }

    private static void SetVisiblePoints(this Player player, int pointsToSet)
    {
        var scoreDifference = pointsToSet - player.ScoreManager.VisiblePoints;

        player.ScoreManager.AddVisiblePoints(scoreDifference);
    }

    private static void SetHiddenPoints(this Player player, int pointsToSet)
    {
        var scoreDifference = pointsToSet - player.ScoreManager.HiddenPoints;

        player.ScoreManager.AddHiddenPoints(scoreDifference);
    }

    private static void HasRolledSeven(this Game game)
    {
        game.MoveState(ActionType.RollSeven);
        game.PlayerManager.CalculateDiscardRequirements();
        if (!game.PlayerManager.PlayersNeedToDiscard)
        {
            game.MoveState(ActionType.AllResourcesDiscarded);
        }
    }

    private static void HasRolledDice(this Game game)
    {
        game.MoveState(ActionType.RollDice);
    }

    private static void PrepareLongestRoad(this Board board, PlayerColour playerColour)
    {
        var roadLocations = new List<(Point, Point)>
        {
            (new(2, 2), new(3, 2)),
            (new(2, 3), new(1, 3))
        };

        foreach (var (firstPoint, secondPoint) in roadLocations)
        {
            board.PlaceRoad(firstPoint, secondPoint, playerColour);
        }
    }

    private static void PrepareSettlementPlacement(this Board board, PlayerColour playerColour)
    {
        board.PlaceRoad(new(1, 2), new(2, 2), playerColour);
    }

    private static void PrepareLargestArmy(this Game game, PlayerColour playerColour)
    {
        game.CurrentPlayer.AddDevelopmentCard(DevelopmentCardType.Knight);
        game.CurrentPlayer.AddDevelopmentCard(DevelopmentCardType.Knight);
        game.PlayKnightCard();
        game.MoveState(ActionType.MoveRobber);
        game.MoveState(ActionType.StealResource);
        do
        {
            game.MoveState(ActionType.RollDice);
            game.EndTurn();
        }
        while (game.CurrentPlayerColour != playerColour);

        game.PlayKnightCard();
        game.MoveState(ActionType.MoveRobber);
        game.MoveState(ActionType.StealResource);
        do
        {
            game.MoveState(ActionType.RollDice);
            game.EndTurn();
        }
        while (game.CurrentPlayerColour != playerColour);
    }
}
