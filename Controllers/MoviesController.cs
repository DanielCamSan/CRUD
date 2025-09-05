using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;
using static newCRUD.Models.Movie;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[v1]")]
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
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie);
        }
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
                Genre = dto.Genre.Trim(),
                Year = dto.Year
            };

            _movies[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _movies.RemoveAll(m => m.Id == id);
            return removed == 0 ? NotFound(new { error = "Movie not found", status = 404 }) : NoContent();
        }

    }
}