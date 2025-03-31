using Rocketer.Enums;

namespace Rocketer.Dtos;

public class LaunchDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime LaunchDate { get; set; }
    public LaunchStatus Status { get; set; }
}