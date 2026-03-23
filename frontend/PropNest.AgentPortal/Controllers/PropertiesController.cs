using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PropNest.AgentPortal.Models;

namespace PropNest.AgentPortal.Controllers;

public class PropertiesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PropertiesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient GetPropertyClient()
    {
        var client = _httpClientFactory.CreateClient("PropertyService");
        var token = HttpContext.Session.GetString("Token");
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private IActionResult RedirectIfNotLoggedIn()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            return RedirectToAction("Login", "Account");
        return null!;
    }

    public async Task<IActionResult> Index()
    {
        var redirect = RedirectIfNotLoggedIn();
        if (redirect != null) return redirect;

        var client = GetPropertyClient();
        var properties = await client.GetFromJsonAsync<List<PropertyViewModel>>(
    "/api/Properties/my-listings");

    ViewBag.UserName = HttpContext.Session.GetString("UserName");
    return View(properties ?? []);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var redirect = RedirectIfNotLoggedIn();
        if (redirect != null) return redirect;

        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return View(new CreatePropertyViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePropertyViewModel model)
    {
        var client = GetPropertyClient();
        var response = await client.PostAsJsonAsync("/api/Properties", model);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        ViewBag.Error = "Failed to create listing. Please try again.";
        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = GetPropertyClient();
        await client.DeleteAsync($"/api/Properties/{id}");
        return RedirectToAction("Index");
    }
}

