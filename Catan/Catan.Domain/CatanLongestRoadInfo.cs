using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed record CatanLongestRoadInfo(CatanPlayerColour Colour, int Length);
