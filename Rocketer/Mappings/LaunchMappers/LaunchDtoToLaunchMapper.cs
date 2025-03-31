using Rocketer.Data.Models.Database;
using Rocketer.Dtos;

namespace Rocketer.Mappings.LaunchMappers;

public static class LaunchDtoToLaunchMapper
{
    public static Launch ToLaunch(this LaunchDto launchDto)
    {
        return new Launch
        {
            Id = launchDto.Id,
            LaunchDate = launchDto.LaunchDate,
            Status = launchDto.Status,
            
        };
    }
}