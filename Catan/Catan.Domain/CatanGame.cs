using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanGame
{
    private readonly List<CatanPlayer> players = new();
    private readonly List<int> rolledDice = new();

    public CatanGame(int numberOfPlayers)
    {
        Board = new CatanBoard();
        players = new List<CatanPlayer>();

        RollDice();
        InitialisePlayers(numberOfPlayers);
    }

    public CatanBoard Board { get; private set; } = new();

    public int DiceTotal => rolledDice.Sum();

    public List<CatanPlayer> GetPlayers() => players;

    public List<int> GetRolledDice() => rolledDice;

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

    private void RollDice()
    {
        rolledDice.Clear();
        rolledDice.AddRange(DiceRoller.RollDice(2, 6));
    }
}
