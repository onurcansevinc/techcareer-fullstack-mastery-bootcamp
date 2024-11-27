using System;
using techcareer_fullstack_mastery_bootcamp.Models;

namespace techcareer_fullstack_mastery_bootcamp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public int Rating { get; set; }  // 1-5 arası
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        
        public Book Book { get; set; }
    }
} 