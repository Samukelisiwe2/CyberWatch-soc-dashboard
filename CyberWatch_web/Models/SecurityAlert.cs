using System.ComponentModel.DataAnnotations;

namespace CyberWatch_web.Models;

public enum AlertSeverity
{
    Low,
    Medium,
    High
}

public enum AlertStatus
{
    Open,
    Investigating,
    Resolved
}

public class SecurityAlert
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string AlertType { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(45)]
    public string IpAddress { get; set; } = string.Empty;

    public AlertSeverity Severity { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public AlertStatus Status { get; set; } = AlertStatus.Open;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}