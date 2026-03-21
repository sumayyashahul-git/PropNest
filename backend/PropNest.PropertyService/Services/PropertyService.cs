using Microsoft.EntityFrameworkCore;
using PropNest.PropertyService.Data;
using PropNest.PropertyService.Models;
using PropNest.Shared.Events;
using PropNest.Shared.Services;

namespace PropNest.PropertyService.Services;

public interface IPropertyService
{
    Task<(List<Property> Items, int Total)> SearchAsync(
        string? location, PropertyType? type,
        ListingType? listingType, decimal? minPrice,
        decimal? maxPrice, int? minBedrooms, int page);

    Task<Property?> GetByIdAsync(Guid id);

    Task<Property> CreateAsync(
        CreatePropertyRequest request,
        string agentId,
        string agentName);

    Task<Property?> UpdateAsync(
        Guid id,
        UpdatePropertyRequest request,
        string agentId);

    Task<bool> DeleteAsync(Guid id, string agentId);

    Task<List<Property>> GetByAgentAsync(string agentId);
}

public class PropertyService : IPropertyService
{
    private readonly PropertyDbContext _db;
    private readonly IEventBus _eventBus;

    public PropertyService(PropertyDbContext db, IEventBus eventBus)
    {
        _db = db;
        _eventBus = eventBus;
    }

    public async Task<(List<Property> Items, int Total)> SearchAsync(
        string? location, PropertyType? type,
        ListingType? listingType, decimal? minPrice,
        decimal? maxPrice, int? minBedrooms, int page)
    {
        var query = _db.Properties
            .Where(p => p.Status == PropertyStatus.Available)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(p =>
                p.Location.Contains(location) ||
                p.Community.Contains(location));

        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);

        if (listingType.HasValue)
            query = query.Where(p => p.ListingType == listingType.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (minBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms >= minBedrooms.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * 12)
            .Take(12)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _db.Properties
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Property> CreateAsync(
        CreatePropertyRequest request,
        string agentId,
        string agentName)
    {
        var property = new Property
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            ServiceCharge = request.ServiceCharge,
            Location = request.Location,
            Community = request.Community,
            Emirate = request.Emirate,
            Type = request.Type,
            ListingType = request.ListingType,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            AreaSqFt = request.AreaSqFt,
            HasParking = request.HasParking,
            HasPool = request.HasPool,
            HasGym = request.HasGym,
            IsFurnished = request.IsFurnished,
            AgentId = agentId,
            AgentName = agentName
        };

        _db.Properties.Add(property);
        await _db.SaveChangesAsync();

        _eventBus.Publish(new PropertyListedEvent(
            property.Id,
            property.Title,
            property.Price,
            property.Location,
            agentId,
            property.CreatedAt
        ));

        return property;
    }

    public async Task<Property?> UpdateAsync(
        Guid id,
        UpdatePropertyRequest request,
        string agentId)
    {
        var property = await _db.Properties
            .FirstOrDefaultAsync(p => p.Id == id 
                                   && p.AgentId == agentId);

        if (property is null)
            return null;

        var oldPrice = property.Price;

        if (request.Title is not null)
            property.Title = request.Title;

        if (request.Description is not null)
            property.Description = request.Description;

        if (request.Status.HasValue)
            property.Status = request.Status.Value;

        if (request.Price.HasValue && request.Price.Value != oldPrice)
        {
            property.Price = request.Price.Value;

            if (request.Price.Value < oldPrice)
            {
                _eventBus.Publish(new PriceDroppedEvent(
                    property.Id,
                    property.Title,
                    oldPrice,
                    request.Price.Value,
                    DateTime.UtcNow
                ));
            }
        }

        property.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return property;
    }

    public async Task<bool> DeleteAsync(Guid id, string agentId)
    {
        var property = await _db.Properties
            .FirstOrDefaultAsync(p => p.Id == id 
                                   && p.AgentId == agentId);

        if (property is null)
            return false;

        _db.Properties.Remove(property);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Property>> GetByAgentAsync(string agentId)
    {
        return await _db.Properties
            .Where(p => p.AgentId == agentId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}