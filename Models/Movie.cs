namespace movie.Areas.Admin.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Des { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime DateTime { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category category { get; set; }

        public int CinemaId { get; set; }
        public Cinema cinema { get; set; }

        public List<MovieSubImages> movieSubImages { get; set; }
    }
}
