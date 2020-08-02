using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}