using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using ECommerce.Repositories; // ✅ مكان الريبو الجينريك

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly Repository<Cinema> _repository;

        public CinemaController(ApplicationDbContext context)
        {
            _repository = new Repository<Cinema>(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var cinemas = await _repository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(cinemas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile? img, CancellationToken cancellationToken)
        {
            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);

                using var stream = System.IO.File.Create(filepath);
                await img.CopyToAsync(stream);
                cinema.Img = filename;
            }

            await _repository.AddAsync(cinema, cancellationToken);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var cinema = await _repository.GetOneAsync(c => c.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (cinema == null)
                return NotFound();

            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile? img, CancellationToken cancellationToken)
        {
            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);

                using var stream = System.IO.File.Create(filepath);
                await img.CopyToAsync(stream);
                cinema.Img = filename;
            }

            _repository.Update(cinema);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var cinema = await _repository.GetOneAsync(c => c.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (cinema == null)
                return NotFound();

            _repository.Delete(cinema);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
