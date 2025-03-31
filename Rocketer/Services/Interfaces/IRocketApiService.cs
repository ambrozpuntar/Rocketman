using Rocketer.Data.Models.Database;
using Rocketer.Dtos;
using Rocketer.Responses;

namespace Rocketer.Services.Interfaces;

public interface IRocketApiService
{
    Task <List<LaunchDto>> GetLaunchesAsync();
}