using System;
using Microsoft.AspNetCore.Mvc;

namespace birds.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}