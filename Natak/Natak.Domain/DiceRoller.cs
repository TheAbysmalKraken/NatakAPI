namespace Natak.Domain;

public static class DiceRoller
{
    private static readonly Random random = new();

    public static DiceRoll RollDice(int numberOfDice, int diceSides)
    {
        List<int> rolledDice = [];

        for (var i = 0; i < numberOfDice; i++)
        {
            var numberRolled = random.Next(diceSides) + 1;
            rolledDice.Add(numberRolled);
        }

        return new(rolledDice);
    }
}
