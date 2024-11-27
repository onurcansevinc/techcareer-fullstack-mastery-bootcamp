using System;
using techcareer_fullstack_mastery_bootcamp.Models;

namespace techcareer_fullstack_mastery_bootcamp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Foreign Keys
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
} 