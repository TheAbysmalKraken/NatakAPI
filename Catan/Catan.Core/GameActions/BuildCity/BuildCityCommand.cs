using Catan.Domain;

namespace Catan.Core.GameActions.BuildCity;

public sealed record BuildCityCommand(string GameId, Point BuildPoint) : ICommand;
