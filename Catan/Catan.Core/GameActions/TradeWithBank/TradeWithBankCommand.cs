namespace Catan.Core.GameActions.TradeWithBank;

public sealed record TradeWithBankCommand(
    string GameId,
    int ResourceToGive,
    int ResourceToGet)
    : ICommand;
