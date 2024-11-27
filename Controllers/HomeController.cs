using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using techcareer_fullstack_mastery_bootcamp.Models;
using techcareer_fullstack_mastery_bootcamp.Data;
using Microsoft.EntityFrameworkCore;

namespace techcareer_fullstack_mastery_bootcamp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext _context;

        public HomeController(ILogger<HomeController> logger, BookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedBooks = await _context.Books
                    .OrderByDescending(b => b.Id)
                    .Take(4)
                    .ToListAsync(),

                RecentBooks = await _context.Books
                    .OrderByDescending(b => b.Id)
                    .Take(8)
                    .ToListAsync(),

                GenreBooks = await _context.Books
                    .GroupBy(b => b.Genre)
                    .Select(g => new GenreBookGroup
                    {
                        Genre = g.Key,
                        Books = g.Take(4).ToList()
                    })
                    .Take(3)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
