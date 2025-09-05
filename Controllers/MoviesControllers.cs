using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // <- versión y recurso
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> _movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Inception", Year = 2010, Genre = "Sci-Fi" },
            new Movie { Id = Guid.NewGuid(), Title = "The Godfather", Year = 1972, Genre = "Crime" }
        };

        // GET /api/v1/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAll() => Ok(_movies);

        // GET /api/v1/movies/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(movie);
        }

        // POST /api/v1/movies
        [HttpPost]
        public ActionResult<Movie> Create([FromBody] CreateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Year = dto.Year,
                Genre = dto.Genre.Trim()
            };

            _movies.Add(movie);
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie); // 201 + Location
        }

        // PUT /api/v1/movies/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Movie> Update(Guid id, [FromBody] UpdateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _movies.FindIndex(m => m.Id == id);
            if (index == -1) return NotFound(new { error = "Movie not found", status = 404 });

            var updated = new Movie
            {
                Id = id,
                Title = dto.Title.Trim(),
                Year = dto.Year,
                Genre = dto.Genre.Trim()
            };

            _movies[index] = updated;
            return Ok(updated); // 200
        }

        // DELETE /api/v1/movies/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _movies.RemoveAll(m => m.Id == id);
            return removed == 0 ? NotFound(new { error = "Movie not found", status = 404 })
                                : NoContent(); // 204
        }
    }
}
