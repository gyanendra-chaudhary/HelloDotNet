﻿using Microsoft.AspNetCore.Mvc;

namespace SignalRWithSqlTableDependency.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
