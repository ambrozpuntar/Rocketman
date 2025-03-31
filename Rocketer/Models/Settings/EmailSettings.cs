﻿namespace Rocketer.Models.Settings;

public class EmailSettings
{
    public string MailServer { get; set; } = "";
    public int MailPort { get; set; }
    public string SenderEmail { get; set; } = "";
    public string SenderName { get; set; } = "";
    public string SenderDomain { get; set; } = "";
    public string Password { get; set; } = "";
    public List<string> Recipents { get; set; } = new List<string>();
}