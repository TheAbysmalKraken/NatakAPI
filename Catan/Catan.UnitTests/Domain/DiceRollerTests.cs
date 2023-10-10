using Catan.Domain;

namespace Catan.UnitTests.Domain
{
    public class DiceRollerTests
    {
        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 8)]
        [InlineData(2, 9)]
        [InlineData(400, 2)]
        [InlineData(999, 3)]
        public void RollDice_CorrectNumberOfDiceRolled(int numberOfDice, int diceSides)
        {
            // Act
            var rolledDice = DiceRoller.RollDice(numberOfDice, diceSides);

            // Assert
            int rolledDiceCount = rolledDice.Count;
            Assert.Equal(rolledDiceCount, numberOfDice);
        }

        [Fact]
        public void RollDice_AllDiceValuesWithinMaxSides()
        {
            // Arrange
            int numberOfDice = 100000;
            int diceSides = 6;

            // Act
            var rolledDice = DiceRoller.RollDice(numberOfDice, diceSides);

            // Assert
            foreach (var dice in rolledDice)
            {
                Assert.InRange(dice, 1, diceSides);
            }
        }
    }
}
