using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> _movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Inception",   Genre = "Sci-Fi", Year = 2010 },
            new Movie { Id = Guid.NewGuid(), Title = "The Godfather", Genre = "Crime",  Year = 1972 }

        };

        // READ: GET api/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAll()
            => Ok(_movies);

        // READ: GET api/movies/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(a => a.Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(movie);
        }

        // CREATE: POST api/movies
        [HttpPost]
        public ActionResult<Movie> Create([FromBody] CreateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Genre = dto.Genre.Trim(),
                Year = dto.Year
            };

            _movies.Add(movie);
            // 201 Created + Location header
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie);
        }
    }
}