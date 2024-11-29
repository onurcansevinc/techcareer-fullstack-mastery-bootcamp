using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using techcareer_fullstack_mastery_bootcamp.Models;
using techcareer_fullstack_mastery_bootcamp.Data;

namespace BookStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly BookContext _context;

        public ReviewController(BookContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Books", new { id = review.BookId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string comment, int rating)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            review.Comment = comment;
            review.Rating = rating;
            review.CreatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Books", new { id = review.BookId });
        }
    }
} 