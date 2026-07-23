using CyberWatch_web.Data;
using CyberWatch_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberWatch_web.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalAlerts = await _context.SecurityAlerts.CountAsync(),

            OpenAlerts = await _context.SecurityAlerts
                .CountAsync(alert => alert.Status == AlertStatus.Open),

            HighSeverityAlerts = await _context.SecurityAlerts
                .CountAsync(alert => alert.Severity == AlertSeverity.High),

            TotalSecurityLogs = await _context.SecurityLogs.CountAsync(),

            RecentAlerts = await _context.SecurityAlerts
                .OrderByDescending(alert => alert.CreatedAtUtc)
                .Take(10)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var alert = await _context.SecurityAlerts
            .FirstOrDefaultAsync(a => a.Id == id);

        if (alert is null)
        {
            return NotFound();
        }

        return View(alert);
    }
}