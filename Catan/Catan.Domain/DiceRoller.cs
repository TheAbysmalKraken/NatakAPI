namespace Catan.Domain
{
    public static class DiceRoller
    {
        private static readonly Random random = new Random();

        public static List<int> RollDice(int numberOfDice, int diceSides)
        {
            List<int> rolledDice = new();

            for (var i = 0; i < numberOfDice; i++)
            {
                var numberRolled = random.Next(diceSides);
                rolledDice.Add(numberRolled);
            }

            return rolledDice;
        }
    }
}
