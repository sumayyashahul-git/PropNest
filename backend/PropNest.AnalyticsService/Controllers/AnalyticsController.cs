using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropNest.AnalyticsService.Services;

namespace PropNest.AnalyticsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _service;

    public AnalyticsController(IAnalyticsService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Dashboard()
    {
        var stats = await _service.GetDashboardStatsAsync();
        return Ok(stats);
    }

    [HttpGet("top-viewed")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TopViewed([FromQuery] int count = 10)
    {
        var result = await _service.GetTopViewedAsync(count);
        return Ok(result);
    }

    [HttpGet("recent-listings")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RecentListings([FromQuery] int count = 10)
    {
        var result = await _service.GetRecentListingsAsync(count);
        return Ok(result);
    }

    [HttpGet("price-history/{propertyId:guid}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> PriceHistory(Guid propertyId)
    {
        var result = await _service.GetPriceHistoryAsync(propertyId);
        return Ok(result);
    }

    [HttpPost("property-listed")]
    public async Task<IActionResult> PropertyListed([FromBody] PropertyListedRequest request)
    {
        await _service.PropertyListedAsync(
            request.PropertyId, request.Title,
            request.Price, request.Location,
            request.AgentId, request.ListedAt);
        return Ok();
    }

    [HttpPost("property-viewed")]
    public async Task<IActionResult> PropertyViewed([FromBody] PropertyViewedRequest request)
    {
        await _service.PropertyViewedAsync(request.PropertyId);
        return Ok();
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "Analytics Service is running", timestamp = DateTime.UtcNow });
    }
}

public record PropertyListedRequest(
    Guid PropertyId, string Title, decimal Price,
    string Location, string AgentId, DateTime ListedAt);

public record PropertyViewedRequest(Guid PropertyId);