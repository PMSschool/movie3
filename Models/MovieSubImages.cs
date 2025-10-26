using Microsoft.EntityFrameworkCore;

namespace movie.Areas.Admin.Models
{
    [PrimaryKey(nameof(MovieId), nameof(Img))]
    public class MovieSubImages
    {
        public int MovieId { get; set; }
        public Movie movie { get; } = null!;
        public string Img { get; set; } = string.Empty;
    }
}
