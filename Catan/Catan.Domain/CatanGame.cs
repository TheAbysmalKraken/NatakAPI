using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public class CatanGame
    {
        private CatanBoard board;
        private List<CatanPlayer> players;
        private List<int> rolledDice = new();
        private int diceTotal;

        public CatanGame(int numberOfPlayers)
        {
            board = new CatanBoard();
            players = new List<CatanPlayer>();

            RollDice();
            players = InitialisePlayers(numberOfPlayers);
        }

        private List<CatanPlayer> InitialisePlayers(int numberOfPlayers)
        {
            List<CatanPlayer> newPlayers = new();

            for (var i = 0; i < numberOfPlayers; i++)
            {
                CatanPlayerColour playerColour = (CatanPlayerColour)(i + 1);
                var newPlayer = new CatanPlayer(playerColour);

                newPlayers.Add(newPlayer);
            }

            return newPlayers;
        }

        private void RollDice()
        {
            rolledDice = DiceRoller.RollDice(2, 6);
            diceTotal = rolledDice.Sum();
        }
    }
}
