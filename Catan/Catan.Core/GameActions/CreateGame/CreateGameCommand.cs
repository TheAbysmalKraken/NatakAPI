using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.CreateGame;

public sealed record CreateGameCommand(
    int PlayerCount,
    int? Seed = null) : ICommand<CreateGameResponse>;
