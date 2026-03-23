using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PropNest.AgentPortal.Models;

namespace PropNest.AgentPortal.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient("AuthService");

        var response = await client.PostAsJsonAsync("/api/Auth/login", new
        {
            email = model.Email,
            password = model.Password
        });

        if (!response.IsSuccessStatusCode)
        {
            model.ErrorMessage = "Invalid email or password.";
            return View(model);
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result?.Role != "Agent" && result?.Role != "Admin")
        {
            model.ErrorMessage = "Access denied. Agent account required.";
            return View(model);
        }

        HttpContext.Session.SetString("Token", result.Token);
        HttpContext.Session.SetString("UserName", result.FullName);
        HttpContext.Session.SetString("UserRole", result.Role);

        return RedirectToAction("Index", "Properties");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

public record AuthResponse(
    string Token,
    string FullName,
    string Email,
    string Role,
    DateTime ExpiresAt
);