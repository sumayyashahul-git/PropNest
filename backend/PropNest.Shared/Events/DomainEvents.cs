namespace PropNest.Shared.Events;

public record PropertyListedEvent(
    Guid PropertyId,
    string Title,
    decimal Price,
    string Location,
    string AgentId,
    DateTime ListedAt
);
 
public record PropertyViewedEvent(
    Guid PropertyId,
    string UserId,
    DateTime ViewedAt
);

public record PriceDroppedEvent(
    Guid PropertyId,
    string Title,
    decimal OldPrice,
    decimal NewPrice,
    DateTime DroppedAt
);