using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using movie.ViewModel;
using ECommerce.Repositories;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly Repository<Movie> _movieRepo;
        private readonly Repository<MovieSubImages> _subImgRepo;
        private readonly Repository<Category> _categoryRepo;
        private readonly Repository<Cinema> _cinemaRepo;
        private readonly IWebHostEnvironment _env;

        public MovieController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _movieRepo = new Repository<Movie>(context);
            _subImgRepo = new Repository<MovieSubImages>(context);
            _categoryRepo = new Repository<Category>(context);
            _cinemaRepo = new Repository<Cinema>(context);
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var movies = await _movieRepo
                .GetAsync(
                    tracked: false,
                    include: q => q
                        .Include(m => m.category)
                        .Include(m => m.cinema),
                    cancellationToken: cancellationToken
                );

            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepo.GetAsync(tracked: false, cancellationToken: cancellationToken);
            var cinemas = await _cinemaRepo.GetAsync(tracked: false, cancellationToken: cancellationToken);

            return View(new movieVM
            {
                categories = categories,
                cinemas = cinemas
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImg, List<IFormFile> SubImages, CancellationToken cancellationToken)
        {
            // حفظ الصورة الرئيسية
            if (MainImg is not null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await MainImg.CopyToAsync(stream);
                }

                movie.MainImg = fileName;
            }

            await _movieRepo.AddAsync(movie, cancellationToken);
            await _movieRepo.CommitAsync(cancellationToken);

            // حفظ الصور الفرعية
            if (SubImages is not null && SubImages.Count > 0)
            {
                var subFolder = Path.Combine(_env.WebRootPath, "uploads", "movies");
                if (!Directory.Exists(subFolder))
                    Directory.CreateDirectory(subFolder);

                foreach (var img in SubImages)
                {
                    var subFileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var subFilePath = Path.Combine(subFolder, subFileName);

                    using (var stream = System.IO.File.Create(subFilePath))
                    {
                        await img.CopyToAsync(stream);
                    }

                    await _subImgRepo.AddAsync(new MovieSubImages
                    {
                        Img = subFileName,
                        MovieId = movie.Id
                    }, cancellationToken);
                }

                await _subImgRepo.CommitAsync(cancellationToken);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepo.GetOneAsync(m => m.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (movie == null)
                return NotFound();

            var categories = await _categoryRepo.GetAsync(tracked: false, cancellationToken: cancellationToken);
            var cinemas = await _cinemaRepo.GetAsync(tracked: false, cancellationToken: cancellationToken);

            return View(new movieVM
            {
                categories = categories,
                cinemas = cinemas,
                movie = movie
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie, IFormFile MainImg, List<IFormFile> SubImages, CancellationToken cancellationToken)
        {
            // تحديث الصورة الرئيسية
            if (MainImg is not null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await MainImg.CopyToAsync(stream);
                }

                movie.MainImg = fileName;
            }

            // تحديث الصور الفرعية
            if (SubImages is not null && SubImages.Count > 0)
            {
                var oldImages = await _subImgRepo.GetAsync(s => s.MovieId == movie.Id, tracked: true, cancellationToken: cancellationToken);

                // حذف القديمة من المجلد
                foreach (var img in oldImages)
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "uploads", "movies", img.Img);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                _subImgRepo.DeleteRange(oldImages);
                await _subImgRepo.CommitAsync(cancellationToken);

                var subFolder = Path.Combine(_env.WebRootPath, "uploads", "movies");
                if (!Directory.Exists(subFolder))
                    Directory.CreateDirectory(subFolder);

                foreach (var img in SubImages)
                {
                    var newFileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var newFilePath = Path.Combine(subFolder, newFileName);

                    using (var stream = System.IO.File.Create(newFilePath))
                    {
                        await img.CopyToAsync(stream);
                    }

                    await _subImgRepo.AddAsync(new MovieSubImages
                    {
                        Img = newFileName,
                        MovieId = movie.Id
                    }, cancellationToken);
                }

                await _subImgRepo.CommitAsync(cancellationToken);
            }

            _movieRepo.Update(movie);
            await _movieRepo.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepo.GetOneAsync(m => m.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (movie == null)
                return NotFound();

            // حذف الصورة الرئيسية
            var mainPath = Path.Combine(_env.WebRootPath, "uploads", movie.MainImg ?? "");
            if (System.IO.File.Exists(mainPath))
                System.IO.File.Delete(mainPath);

            // حذف الصور الفرعية
            var subImgs = await _subImgRepo.GetAsync(s => s.MovieId == id, tracked: true, cancellationToken: cancellationToken);
            foreach (var img in subImgs)
            {
                var imgPath = Path.Combine(_env.WebRootPath, "uploads", "movies", img.Img);
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }
            _subImgRepo.DeleteRange(subImgs);
            await _subImgRepo.CommitAsync(cancellationToken);

            _movieRepo.Delete(movie);
            await _movieRepo.CommitAsync(cancellationToken);

            TempData["success-notification"] = "Movie deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
