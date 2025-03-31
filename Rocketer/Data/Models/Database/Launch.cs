using Rocketer.Enums;

namespace Rocketer.Data.Models.Database;
public class Launch
{
    public string? Id { get; set; }
    public DateTime LaunchDate { get; set; }
    public LaunchStatus Status { get; set; }
    public NotifiedStatus NotifiedStatus { get; set; }
}