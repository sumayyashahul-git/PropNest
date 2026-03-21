using System.ComponentModel.DataAnnotations;

namespace PropNest.PropertyService.Models;

public class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal? ServiceCharge { get; set; }

    [Required, MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Community { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Emirate { get; set; } = "Dubai";

    public PropertyType Type { get; set; }
    public ListingType ListingType { get; set; }
    public PropertyStatus Status { get; set; } = PropertyStatus.Available;

    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public double AreaSqFt { get; set; }

    public bool HasParking { get; set; }
    public bool HasPool { get; set; }
    public bool HasGym { get; set; }
    public bool IsFurnished { get; set; }

    public string ImageUrls { get; set; } = string.Empty;

    public string AgentId { get; set; } = string.Empty;
    public string AgentName { get; set; } = string.Empty;

    public int ViewCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum PropertyType 
{ 
    Apartment, 
    Villa, 
    Townhouse, 
    Penthouse, 
    Studio, 
    Office 
}

public enum ListingType 
{ 
    Sale, 
    Rent 
}

public enum PropertyStatus 
{ 
    Available, 
    UnderOffer, 
    Sold, 
    Rented 
}

public record CreatePropertyRequest(
    string Title,
    string Description,
    decimal Price,
    decimal? ServiceCharge,
    string Location,
    string Community,
    string Emirate,
    PropertyType Type,
    ListingType ListingType,
    int Bedrooms,
    int Bathrooms,
    double AreaSqFt,
    bool HasParking,
    bool HasPool,
    bool HasGym,
    bool IsFurnished
);

public record UpdatePropertyRequest(
    string? Title,
    string? Description,
    decimal? Price,
    PropertyStatus? Status
);