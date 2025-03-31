using System.ComponentModel;

namespace Rocketer.Enums;

public enum LaunchStatus
{
    [Description("Unknown")]
    Unknown = 0,

    [Description("Launch")]
    Launch = 1,

    [Description("Delay")]
    Delay = 2,

    [Description("On hold")]
    OnHold = 3,

}

public enum NotifiedStatus
{
    [Description("Unknown")]
    Unknown = 0,
    [Description("Notified")]
    Notified = 1,
    [Description("Not Notified")]
    NotNotified = 2,
}