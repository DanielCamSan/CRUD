using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> _movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "The Matrix", Genre = "Sci-Fi", Year = 1999 },
            new Movie { Id = Guid.NewGuid(), Title = "Spirited Away", Genre = "Animation", Year = 2001 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAll()
            => Ok(_movies);

        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            return movie is null ? NotFound(new { error = "Movie not found", status = 404 }) : Ok(movie);
        }
    }
}