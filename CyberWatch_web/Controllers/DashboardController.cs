using CyberWatch_web.Data;
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
        var alerts = await _context.SecurityAlerts
            .OrderByDescending(a => a.CreatedAtUtc)
            .ToListAsync();

        return View(alerts);
    }
}