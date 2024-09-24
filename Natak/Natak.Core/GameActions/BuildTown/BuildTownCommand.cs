using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.BuildTown;

public sealed record BuildTownCommand(string GameId, Point BuildPoint) : ICommand;
