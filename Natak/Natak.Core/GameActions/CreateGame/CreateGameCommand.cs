using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.CreateGame;

public sealed record CreateGameCommand(
    int PlayerCount,
    int? Seed = null) : ICommand<CreateGameResponse>;
