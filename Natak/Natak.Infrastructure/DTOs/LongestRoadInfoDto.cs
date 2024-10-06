using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class LongestRoadInfoDto : IDto<LongestRoadInfo, LongestRoadInfoDto>
{
    public required PlayerColour Colour { get; init; }
    
    public required int Length { get; init; }
    
    public static LongestRoadInfoDto FromDomain(LongestRoadInfo domain)
    {
        return new LongestRoadInfoDto()
        {
            Colour = domain.Colour,
            Length = domain.Length
        };
    }

    public LongestRoadInfo ToDomain()
    {
        return new LongestRoadInfo(Colour, Length);
    }
}