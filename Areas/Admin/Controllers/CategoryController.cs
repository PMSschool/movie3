using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using ECommerce.Repositories;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly Repository<Category> _repository;

        public CategoryController(ApplicationDbContext context)
        {
            _repository = new Repository<Category>(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _repository.AddAsync(category, cancellationToken);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var category = await _repository.GetOneAsync(c => c.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(category);

            _repository.Update(category);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _repository.GetOneAsync(c => c.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (category == null)
                return NotFound();

            _repository.Delete(category);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
