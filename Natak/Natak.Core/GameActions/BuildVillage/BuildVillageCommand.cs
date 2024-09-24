using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.BuildVillage;

public sealed record BuildVillageCommand(string GameId, Point BuildPoint) : ICommand;
