using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using ECommerce.Repositories;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly Repository<Actor> _repository;
        private readonly IWebHostEnvironment _env;

        public ActorController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _repository = new Repository<Actor>(context);
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var actors = await _repository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(actors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Actor actor, IFormFile img, CancellationToken cancellationToken)
        {
            if (img is not null && img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                }

                actor.Img = fileName;
            }

            await _repository.AddAsync(actor, cancellationToken);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var actor = await _repository.GetOneAsync(a => a.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (actor == null)
                return NotFound();

            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile img, CancellationToken cancellationToken)
        {
            if (img is not null && img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                }

                actor.Img = fileName;
            }

            _repository.Update(actor);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var actor = await _repository.GetOneAsync(a => a.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (actor == null)
                return NotFound();

            _repository.Delete(actor);
            await _repository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
