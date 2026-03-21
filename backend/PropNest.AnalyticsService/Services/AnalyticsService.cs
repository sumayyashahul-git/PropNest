using Microsoft.EntityFrameworkCore;
using PropNest.AnalyticsService.Data;
using PropNest.AnalyticsService.Models;

namespace PropNest.AnalyticsService.Services;

public interface IAnalyticsService
{
    Task PropertyListedAsync(Guid propertyId, string title,
        decimal price, string location, string agentId, DateTime listedAt);

    Task PropertyViewedAsync(Guid propertyId);

    Task PriceDroppedAsync(Guid propertyId, string title,
        decimal oldPrice, decimal newPrice);

    Task<List<PropertyAnalytics>> GetTopViewedAsync(int count = 10);
    Task<List<PropertyAnalytics>> GetRecentListingsAsync(int count = 10);
    Task<List<PriceHistory>> GetPriceHistoryAsync(Guid propertyId);
    Task<object> GetDashboardStatsAsync();
}

public class AnalyticsService : IAnalyticsService
{
    private readonly AnalyticsDbContext _db;

    public AnalyticsService(AnalyticsDbContext db)
    {
        _db = db;
    }

    public async Task PropertyListedAsync(Guid propertyId, string title,
        decimal price, string location, string agentId, DateTime listedAt)
    {
        var existing = await _db.PropertyAnalytics
            .FirstOrDefaultAsync(p => p.PropertyId == propertyId);

        if (existing is not null) return;

        _db.PropertyAnalytics.Add(new PropertyAnalytics
        {
            PropertyId = propertyId,
            Title = title,
            Location = location,
            AgentId = agentId,
            ListedPrice = price,
            CurrentPrice = price,
            ListedAt = listedAt
        });

        await _db.SaveChangesAsync();
    }

    public async Task PropertyViewedAsync(Guid propertyId)
    {
        var analytics = await _db.PropertyAnalytics
            .FirstOrDefaultAsync(p => p.PropertyId == propertyId);

        if (analytics is null) return;

        analytics.ViewCount++;
        analytics.LastUpdated = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task PriceDroppedAsync(Guid propertyId, string title,
        decimal oldPrice, decimal newPrice)
    {
        var analytics = await _db.PropertyAnalytics
            .FirstOrDefaultAsync(p => p.PropertyId == propertyId);

        if (analytics is not null)
        {
            analytics.CurrentPrice = newPrice;
            analytics.LastUpdated = DateTime.UtcNow;
        }

        _db.PriceHistories.Add(new PriceHistory
        {
            PropertyId = propertyId,
            OldPrice = oldPrice,
            NewPrice = newPrice,
            Difference = oldPrice - newPrice
        });

        await _db.SaveChangesAsync();
    }

    public Task<List<PropertyAnalytics>> GetTopViewedAsync(int count = 10)
    {
        return _db.PropertyAnalytics
            .OrderByDescending(p => p.ViewCount)
            .Take(count)
            .ToListAsync();
    }

    public Task<List<PropertyAnalytics>> GetRecentListingsAsync(int count = 10)
    {
        return _db.PropertyAnalytics
            .OrderByDescending(p => p.ListedAt)
            .Take(count)
            .ToListAsync();
    }

    public Task<List<PriceHistory>> GetPriceHistoryAsync(Guid propertyId)
    {
        return _db.PriceHistories
            .Where(p => p.PropertyId == propertyId)
            .OrderByDescending(p => p.ChangedAt)
            .ToListAsync();
    }

    public async Task<object> GetDashboardStatsAsync()
    {
        var totalProperties = await _db.PropertyAnalytics.CountAsync();
        var totalViews = await _db.PropertyAnalytics.SumAsync(p => p.ViewCount);
        var totalPriceDrops = await _db.PriceHistories.CountAsync();
        var avgPrice = await _db.PropertyAnalytics.AverageAsync(p => p.CurrentPrice);

        return new
        {
            totalProperties,
            totalViews,
            totalPriceDrops,
            averagePrice = Math.Round(avgPrice, 2)
        };
    }
}