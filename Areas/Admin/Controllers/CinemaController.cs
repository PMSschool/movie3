using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using System.Collections.Generic;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private ApplicationDbContext _context = new();

        [HttpGet]
        public IActionResult Index()
        {
            var cinema = _context.cinemas.AsQueryable();
            return View(cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile img)
        {
            //_context.cinemas.Add(cinema);
            //_context.SaveChanges();
            //return RedirectToAction("Index", "Cinema");
            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", filename);

                using (var stream = System.IO.File.Create(filepath))
                {
                    img.CopyTo(stream);
                }

                cinema.Img = filename;

            }
            _context.cinemas.Add(cinema);
            _context.SaveChanges();

            return RedirectToAction("Index", "Cinema");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = _context.cinemas.FirstOrDefault(c => c.Id == id);

            return View(cinema);
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile img)
        {
            //_context.cinemas.Update(cinema);
            //_context.SaveChanges();
            //return RedirectToAction("Index", "Cinema");

            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", filename);

                using (var stream = System.IO.File.Create(filepath))
                {
                    img.CopyTo(stream);
                }

                cinema.Img = filename;

            }
            _context.cinemas.Update(cinema);
            _context.SaveChanges();

            return RedirectToAction("Index", "Cinema");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cinema = _context.cinemas.FirstOrDefault(c => c.Id == id);
            _context.cinemas.Remove(cinema);
            _context.SaveChanges();
            return RedirectToAction("Index", "Cinema");
        }
    }
}