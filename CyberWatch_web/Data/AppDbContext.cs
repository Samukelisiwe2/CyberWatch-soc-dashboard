using CyberWatch.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberWatch.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SecurityLog> SecurityLogs => Set<SecurityLog>();

    public DbSet<SecurityAlert> SecurityAlerts => Set<SecurityAlert>();
}