using Microsoft.Extensions.Options;
using Rocketer.Data.Models.Database;
using Rocketer.Dtos;
using Rocketer.Models.Settings;
using Rocketer.Responses;
using Rocketer.Services.Interfaces;
using System.Text.Json;
using Rocketer.Mappings.LaunchMappers;

namespace Rocketer.Services;

public class RocketApiService : IRocketApiService
{
    private readonly HttpClient _httpClient;
    private readonly RocketApiSettings _settings;

    public RocketApiService(
        HttpClient httpClient,
        IOptions<RocketApiSettings> settings
    )
    {
        _httpClient = httpClient;
        _settings = settings.Value;

        _httpClient.BaseAddress = new Uri($"{_settings.BaseUrl}/{_settings.Version}/");
        _httpClient.DefaultRequestHeaders.Add("accept", "application/json");

        if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("Authentication", _settings.ApiKey);
        }
    }

    public async Task<List<LaunchDto>> GetLaunchesAsync()
    {
        var count = await GetLaunchesCountAsync();
        var launchesResult = new List<LaunchNormal>();

        var offset = 0;

        while (offset < count)
        {
            var request = await _httpClient.GetAsync($"launches/upcoming/?hide_recent_previous=true&limit=100&offset={offset}");

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                var launches = JsonSerializer.Deserialize<LaunchesResponse>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (launches is not null)
                {
                    launchesResult.AddRange(launches.Launches.Where(x => x.Status.Id == (int)Enums.LaunchStatus.Launch || x.Status.Id == (int)Enums.LaunchStatus.OnHold));
                }
            }
            offset += 100;
        }

        return launchesResult.Select(x => x.ToLaunchDto()).ToList();
    }

    private async Task<int> GetLaunchesCountAsync()
    {
        var request = await _httpClient.GetAsync($"launches/upcoming/?format=json&hide_recent_previous=true&limit=1&mode=normal");

        if(request.IsSuccessStatusCode)
        {
            var response = await request.Content.ReadAsStringAsync();
            var launchesResponse = JsonSerializer.Deserialize<LaunchesResponse>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if(launchesResponse == null)
            {
                return 0;
            }

            return launchesResponse.Count;
        }

        return 0;
    }
}