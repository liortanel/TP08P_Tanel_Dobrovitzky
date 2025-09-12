using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP08_Tanel_Dobrovitzky.Models;

namespace TP08_Tanel_Dobrovitzky.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
