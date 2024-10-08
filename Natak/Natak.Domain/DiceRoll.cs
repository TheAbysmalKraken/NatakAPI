namespace Natak.Domain;

public sealed record DiceRoll(List<int> Outcome)
{
    public int Total => Outcome.Sum();
}
