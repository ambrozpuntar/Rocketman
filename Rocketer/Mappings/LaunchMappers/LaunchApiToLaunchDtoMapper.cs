using Rocketer.Dtos;
using Rocketer.Responses;

namespace Rocketer.Mappings.LaunchMappers;

public static class LaunchApiToLaunchDtoMapper
{
    public static LaunchDto ToLaunchDto(this LaunchNormal response)
    {
        return new LaunchDto
        {
            Id = response.Id,
            Name = response.Name,
            Status = response.Status.Id switch
            {
                1 => Enums.LaunchStatus.Launch,
                //5 => Enums.LaunchStatus.Delay,
                5 => Enums.LaunchStatus.OnHold,
                _ => Enums.LaunchStatus.Unknown
            },
            LaunchDate = response.LaunchDate
        };
    }
}