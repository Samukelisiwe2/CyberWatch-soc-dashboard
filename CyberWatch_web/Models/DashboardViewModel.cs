namespace CyberWatch_web.Models;

public class DashboardViewModel
{
    public int TotalAlerts { get; set; }

    public int OpenAlerts { get; set; }

    public int HighSeverityAlerts { get; set; }

    public int TotalSecurityLogs { get; set; }

    public List<SecurityAlert> RecentAlerts { get; set; } = [];
}