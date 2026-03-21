using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropNest.PropertyService.Models;
using PropNest.PropertyService.Services;

namespace PropNest.PropertyService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _service;

    public PropertiesController(IPropertyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? location,
        [FromQuery] PropertyType? type,
        [FromQuery] ListingType? listingType,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? minBedrooms,
        [FromQuery] int page = 1)
    {
        var (items, total) = await _service.SearchAsync(
            location, type, listingType,
            minPrice, maxPrice, minBedrooms, page);

        return Ok(new
        {
            items,
            total,
            page,
            totalPages = (int)Math.Ceiling(total / 12.0)
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var property = await _service.GetByIdAsync(id);

        if (property is null)
            return NotFound(new { message = "Property not found." });

        return Ok(property);
    }

    [HttpGet("my-listings")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> MyListings()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var listings = await _service.GetByAgentAsync(agentId);
        return Ok(listings);
    }

    [HttpPost]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePropertyRequest request)
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var agentName = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

        var property = await _service.CreateAsync(
            request, agentId, agentName);

        return CreatedAtAction(
            nameof(GetById),
            new { id = property.Id },
            property);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePropertyRequest request)
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var updated = await _service.UpdateAsync(id, request, agentId);

        if (updated is null)
            return NotFound(new { message = "Property not found." });

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var deleted = await _service.DeleteAsync(id, agentId);

        if (!deleted)
            return NotFound(new { message = "Property not found." });

        return NoContent();
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "Property Service is running",
            timestamp = DateTime.UtcNow
        });
    }
}