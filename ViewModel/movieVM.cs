namespace movie.ViewModel
{
    public class movieVM
    {
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Cinema> cinemas { get; set; }
        public Movie? movie { get; set; }
    }
}
