using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private ApplicationDbContext _context = new();
        private IWebHostEnvironment _env;

        public MovieController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var movies = _context.movies
                .Include(m => m.category)
                .Include(m => m.cinema)
                .ToList();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.categories.ToList();
            ViewBag.Cinemas = _context.cinemas.ToList();
            return View(new Movie());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImgFile, List<IFormFile> SubImgFiles)
        {

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "movies");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            //  Save main image
            if (MainImgFile != null && MainImgFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImgFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await MainImgFile.CopyToAsync(stream);
                }

                movie.MainImg = "/uploads/movies/" + fileName;
            }

            //  Save sub images
            var subImages = new List<string>();
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                foreach (var file in SubImgFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        subImages.Add("/uploads/movies/" + fileName);
                    }
                }
            }

            movie.SubImages = subImages;
            movie.DateTime = DateTime.Now;
            Console.WriteLine($"CategoryId: {movie.CategoryId}");
            Console.WriteLine($"CinemaId: {movie.CinemaId}");
            _context.movies.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) return NotFound();

            ViewBag.Categories = _context.categories.ToList();
            ViewBag.Cinemas = _context.cinemas.ToList();

            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie, IFormFile? MainImgFile, List<IFormFile>? SubImgFiles)
        {
            var existing = _context.movies.FirstOrDefault(x => x.Id == movie.Id);
            if (existing == null) return NotFound();

            existing.Name = movie.Name;
            existing.Des = movie.Des;
            existing.Price = movie.Price;
            existing.Status = movie.Status;
            existing.CategoryId = movie.CategoryId;
            existing.CinemaId = movie.CinemaId;

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "movies");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Replace main image if uploaded
            if (MainImgFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImgFile.FileName);
                string path = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await MainImgFile.CopyToAsync(stream);
                }
                existing.MainImg = "/uploads/movies/" + fileName;
            }

            // Add new sub images if uploaded (preserve existing ones)
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                var newSubImages = new List<string>();
                foreach (var sub in SubImgFiles)
                {
                    if (sub.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sub.FileName);
                        string path = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await sub.CopyToAsync(stream);
                        }
                        newSubImages.Add("/uploads/movies/" + fileName);
                    }
                }

                // Preserve existing sub-images and add new ones
                if (existing.SubImages != null)
                {
                    existing.SubImages.AddRange(newSubImages);
                }
                else
                {
                    existing.SubImages = newSubImages;
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"id is {id}");
            var movie = _context.movies.FirstOrDefault(x => x.Id == id);
            _context.movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index", "Movie", new { area = "Admin" });
        }
    }
}
