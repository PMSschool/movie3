using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace movie.Areas.Admin.Models
{
    [PrimaryKey(nameof(MovieId), nameof(ActorId))]
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
