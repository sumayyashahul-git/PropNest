using Microsoft.EntityFrameworkCore;
using PropNest.AnalyticsService.Models;

namespace PropNest.AnalyticsService.Data;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) 
        : base(options)
    {
    }

    public DbSet<PropertyAnalytics> PropertyAnalytics => Set<PropertyAnalytics>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyAnalytics>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.PropertyId).IsUnique();
            e.HasIndex(p => p.AgentId);
            e.Property(p => p.ListedPrice).HasColumnType("decimal(18,2)");
            e.Property(p => p.CurrentPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PriceHistory>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.PropertyId);
            e.Property(p => p.OldPrice).HasColumnType("decimal(18,2)");
            e.Property(p => p.NewPrice).HasColumnType("decimal(18,2)");
            e.Property(p => p.Difference).HasColumnType("decimal(18,2)");
        });
    }
}