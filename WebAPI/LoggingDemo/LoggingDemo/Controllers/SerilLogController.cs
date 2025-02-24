using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers;

public class SerilLogController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}