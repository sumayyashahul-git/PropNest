namespace PropNest.AnalyticsService.Models;

public class PropertyAnalytics
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropertyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string AgentId { get; set; } = string.Empty;
    public decimal ListedPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public int ViewCount { get; set; } = 0;
    public DateTime ListedAt { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class PriceHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropertyId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal Difference { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}