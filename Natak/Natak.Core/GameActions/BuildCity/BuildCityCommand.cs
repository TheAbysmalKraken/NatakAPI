using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.BuildCity;

public sealed record BuildCityCommand(string GameId, Point BuildPoint) : ICommand;
