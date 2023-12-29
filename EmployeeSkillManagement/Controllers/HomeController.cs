using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeSkillManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeSkillManagement.Controllers;
[Authorize(Policy = "RequireAdminRole")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {var isAuthenticated = User.Identity.IsAuthenticated;

        // Log authentication state
        _logger.LogInformation($"User is authenticated: {isAuthenticated}");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
