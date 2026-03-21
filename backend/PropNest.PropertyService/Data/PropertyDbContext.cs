using Microsoft.EntityFrameworkCore;
using PropNest.PropertyService.Models;

namespace PropNest.PropertyService.Data;

public class PropertyDbContext : DbContext
{
    public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties => Set<Property>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>(e =>
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            e.Property(p => p.ServiceCharge)
                .HasColumnType("decimal(18,2)");

            e.Property(p => p.Title)
                .HasMaxLength(200);

            e.Property(p => p.Location)
                .HasMaxLength(200);

            e.HasIndex(p => p.AgentId);
            e.HasIndex(p => p.Status);
            e.HasIndex(p => p.Type);
        });
    }
}