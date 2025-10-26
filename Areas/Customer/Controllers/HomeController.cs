using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie.ViewModel;
using movie.Areas.Admin.Data;

namespace movie.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index(filterMovieVM filterMovieVM, int page = 1, int pageSize = 6)
        {
            var movies = _context.movies
                .Include(x => x.category)
                .Include(x => x.cinema)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterMovieVM.name))
            {
                movies = movies.Where(e => e.Name.Contains(filterMovieVM.name));
                ViewBag.name = filterMovieVM.name;
            }

            if (filterMovieVM.minPrice != null)
            {
                movies = movies.Where(m => m.Price >= (decimal)filterMovieVM.minPrice.Value);
                ViewBag.minPrice = filterMovieVM.minPrice;
            }

            if (filterMovieVM.maxPrice != null)
            {
                movies = movies.Where(m => m.Price <= (decimal)filterMovieVM.maxPrice.Value);
                ViewBag.maxPrice = filterMovieVM.maxPrice;
            }

            if (filterMovieVM.categoryId != null)
            {
                movies = movies.Where(e => e.CategoryId == filterMovieVM.categoryId);
                ViewBag.categoryId = filterMovieVM.categoryId;
            }

            if (filterMovieVM.cinemaId != null)
            {
                movies = movies.Where(e => e.CinemaId == filterMovieVM.cinemaId);
                ViewBag.cinemaId = filterMovieVM.cinemaId;
            }

            
            int totalItems = movies.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedMovies = movies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Categories = _context.categories.AsEnumerable();
            ViewBag.Cinemas = _context.cinemas.AsEnumerable();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedMovies);
        }
    }
}
