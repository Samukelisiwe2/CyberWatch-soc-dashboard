using System.ComponentModel.DataAnnotations;

namespace CyberWatch_web.Models;

public enum SecurityEventType
{
    LoginFailed,
    LoginSucceeded
}

public class SecurityLog
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(45)]
    public string IpAddress { get; set; } = string.Empty;

    public SecurityEventType EventType { get; set; }

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
