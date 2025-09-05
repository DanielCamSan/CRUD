using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MoviesController:ControllerBase
    {
        private static readonly List<Movie> movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Inception", Genre = "Sci-Fi", Year = 2010 },
            new Movie { Id = Guid.NewGuid(), Title = "La La Land", Genre = "Musical", Year = 2016 },
            new Movie { Id = Guid.NewGuid(), Title = "Spirited Away", Genre = "Animation", Year = 2001 },
            new Movie { Id = Guid.NewGuid(), Title = "Mad Max: Fury Road", Genre = "Action", Year = 2015 }
        };

        // READ: GET api/movies
        [HttpGet]

        public ActionResult<IEnumerable<Movie>> GetAll() { 
           return Ok(movies);
        }

        // READ: GET api/movies/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = movies.FirstOrDefault(a => a. Id == id);
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

            movies.Add(movie);
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie);
        }

        // UPDATE (full): PUT api/movies/{id}
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

        // DELETE: DELETE api/movies/{id}
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
