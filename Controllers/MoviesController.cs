using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers;

{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Inception", Genre = "Action", Year = 2010 },
            new Movie { Id = Guid.NewGuid(), Title = "The Godfather", Genre = "Crime", Year = 1972 }
        };

        // READ: GET api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAll()
            => Ok(movies);

        // READ: GET api/Movies/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var Movie = movies.FirstOrDefault(a => a.Id == id);
            return Movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(Movie);
        }

        // CREATE: POST api/Movies
        [HttpPost]
        public ActionResult<Movie> Create([FromBody] CreateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var Movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Genre = dto.Genre.Trim(),
                Year = dto.Year
            };

            movies.Add(Movie);
            return CreatedAtAction(titleof(GetOne), new { id = Movie.Id }, Movie);
        }

        // UPDATE (full): PUT api/Movies/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Movie> Update(Guid id, [FromBody] UpdateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = movies.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Movie not found", status = 404 });

            var updated = new Movie
            {
                Id = id,
                Title = dto.Title.Trim(),
                Genre = dto.Genre.Trim(),
                Year = dto.Year
            };

            movies[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/Movies/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = movies.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Movie not found", status = 404 })
                : NoContent();
        }
    }
}