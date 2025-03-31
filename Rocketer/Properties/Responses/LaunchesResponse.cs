using System.Text.Json.Serialization;

namespace Rocketer.Responses;

public class LaunchesResponse
{
    public int Count { get; set; }

    [JsonPropertyName("results")]
    public List<LaunchNormal> Launches { get; set; } = new();
}

public class LaunchNormal
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("window_start")]
    public DateTime LaunchDate { get; set; }
    public LaunchStatus Status { get; set; } = new();
}

public class LaunchStatus
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

//Launch Status
/*
    1 Go for Launch
    2 To be determined
    8 To be confirmed
*/