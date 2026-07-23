using CyberWatch_web.Data;
using CyberWatch_web.Models;
using CyberWatch_web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberWatch_web.Controllers;

public class SecurityController : Controller
{
    private readonly AppDbContext _context;
    private readonly ThreatDetectionService _threatDetectionService;

    public SecurityController(
        AppDbContext context,
        ThreatDetectionService threatDetectionService)
    {
        _context = context;
        _threatDetectionService = threatDetectionService;
    }

    [HttpGet]
    public IActionResult Simulate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Simulate(
        string username,
        string ipAddress,
        bool loginSucceeded)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(ipAddress))
        {
            ViewBag.Message = "Username and IP address are required.";
            return View();
        }

        var securityLog = new SecurityLog
        {
            Username = username.Trim(),
            IpAddress = ipAddress.Trim(),
            EventType = loginSucceeded
                ? SecurityEventType.LoginSucceeded
                : SecurityEventType.LoginFailed,
            TimestampUtc = DateTime.UtcNow
        };

        _context.SecurityLogs.Add(securityLog);
        await _context.SaveChangesAsync();

        SecurityAlert? alert = null;

        if (!loginSucceeded)
        {
            alert = await _threatDetectionService.DetectBruteForceAsync(
                securityLog.Username,
                securityLog.IpAddress);
        }

        ViewBag.Message = loginSucceeded
            ? "Successful login recorded."
            : "Failed login recorded.";

        ViewBag.AlertCreated = alert is not null;

        return View();
    }
}
