using Microsoft.AspNetCore.Mvc;

namespace AllSecurity.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}