namespace techcareer_fullstack_mastery_bootcamp.Models
{
    public class HomeViewModel
    {
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> RecentBooks { get; set; }
        public List<GenreBookGroup> GenreBooks { get; set; }
    }

    public class GenreBookGroup
    {
        public string Genre { get; set; }
        public List<Book> Books { get; set; }
    }
} 