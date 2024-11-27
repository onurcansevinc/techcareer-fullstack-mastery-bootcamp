using Microsoft.EntityFrameworkCore;
using techcareer_fullstack_mastery_bootcamp.Models;

namespace techcareer_fullstack_mastery_bootcamp.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
} 