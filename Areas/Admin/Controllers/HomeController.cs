using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;

namespace dashboard_system.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new();
        [Area("Admin")]
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
    }
}
