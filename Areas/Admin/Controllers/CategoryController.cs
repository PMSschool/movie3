using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using movie.Repositories;
using System.Threading.Tasks;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private ApplicationDbContext _context = new();
        private Categoryrepository _categoryrepository = new();

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var category = await _categoryrepository.GetAsync(cancellationToken: cancellationToken);
            return View(category.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            _context.categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
    }
}