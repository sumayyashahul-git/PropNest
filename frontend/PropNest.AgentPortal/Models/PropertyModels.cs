namespace PropNest.AgentPortal.Models;

public class LoginViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}

public class PropertyViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? ServiceCharge { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Community { get; set; } = string.Empty;
    public string Emirate { get; set; } = string.Empty;
    public int Type { get; set; }
    public int ListingType { get; set; }
    public int Status { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public double AreaSqFt { get; set; }
    public bool HasParking { get; set; }
    public bool HasPool { get; set; }
    public bool HasGym { get; set; }
    public bool IsFurnished { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePropertyViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? ServiceCharge { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Community { get; set; } = string.Empty;
    public string Emirate { get; set; } = "Dubai";
    public int Type { get; set; }
    public int ListingType { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public double AreaSqFt { get; set; }
    public bool HasParking { get; set; }
    public bool HasPool { get; set; }
    public bool HasGym { get; set; }
    public bool IsFurnished { get; set; }
}