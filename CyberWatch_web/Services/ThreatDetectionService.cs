using CyberWatch_web.Data;
using CyberWatch_web.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberWatch_web.Services;

public class ThreatDetectionService
{
    private readonly AppDbContext _context;

    public ThreatDetectionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SecurityAlert?> DetectBruteForceAsync(
        string username,
        string ipAddress)
    {
        DateTime cutoffTime = DateTime.UtcNow.AddMinutes(-10);

        int failedAttempts = await _context.SecurityLogs
            .CountAsync(log =>
                log.Username == username &&
                log.IpAddress == ipAddress &&
                log.EventType == SecurityEventType.LoginFailed &&
                log.TimestampUtc >= cutoffTime);

        if (failedAttempts < 5)
        {
            return null;
        }

        bool alertAlreadyExists = await _context.SecurityAlerts
            .AnyAsync(alert =>
                alert.Username == username &&
                alert.IpAddress == ipAddress &&
                alert.AlertType == "Brute Force Attack" &&
                alert.Status != AlertStatus.Resolved);

        if (alertAlreadyExists)
        {
            return null;
        }

        var alert = new SecurityAlert
        {
            AlertType = "Brute Force Attack",
            Username = username,
            IpAddress = ipAddress,
            Severity = AlertSeverity.High,
            Description =
                $"{failedAttempts} failed login attempts were detected " +
                "within the last 10 minutes.",
            Status = AlertStatus.Open,
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.SecurityAlerts.Add(alert);
        await _context.SaveChangesAsync();

        return alert;
    }
}