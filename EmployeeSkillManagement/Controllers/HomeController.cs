using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeSkillManagement.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeSkillManagement.Repository;
using EmployeeSkillManagement.Models.ViewModels;

namespace EmployeeSkillManagement.Controllers;
[Authorize(Policy = "RequireAdminRole")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeRepository _homeRepository;

    public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
    {
        _logger = logger;
        _homeRepository = homeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {   
        var isAuthenticated = User.Identity!.IsAuthenticated;

        HomeViewModel homeViewModel = new HomeViewModel{
            TotalEmployees = await _homeRepository.GetTotalEmployeesCount(),
            TotalSkills = await _homeRepository.GetTotalSkillsCount(),
            SkillWithEmployeeCount = await _homeRepository.GetTopSkillWithEmployeesCount()
        };
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // If it's an AJAX request, return JSON
            return Json(homeViewModel);
        }
        else
        {
            // If it's not an AJAX request, render the view
            return View(homeViewModel);
        }

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
