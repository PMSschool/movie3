using Microsoft.AspNetCore.Mvc;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using System.Collections.Generic;

namespace movie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private ApplicationDbContext _context = new();

        [HttpGet]
        public IActionResult Index()
        {
            var actor = _context.actors.AsQueryable();
            return View(actor.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile img)
        {
            //_context.actors.Add(actor);
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

                actor.Img = filename;

            }
            _context.actors.Add(actor);
            _context.SaveChanges();

            return RedirectToAction("Index", "Actor");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var actor = _context.actors.FirstOrDefault(c => c.Id == id);

            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile img)
        {
            //_context.actors.Update(actor);
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

                actor.Img = filename;

            }
            _context.actors.Update(actor);
            _context.SaveChanges();

            return RedirectToAction("Index", "Actor");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var actor = _context.actors.FirstOrDefault(c => c.Id == id);
            _context.actors.Remove(actor);
            _context.SaveChanges();
            return RedirectToAction("Index", "Actor");
        }
    }
}