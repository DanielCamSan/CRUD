using Microsoft.AspNetCore.Mvc; 

namespace newCRUD.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> _movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Avengers", Genre = "Fiction", Year = 2022 },
            new Movie { Id = Guid.NewGuid(), Title = "The Nun ", Genre = "Horror", Year = 2018 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAll()
        {
            return Ok(_movies);
        }
        
        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(a => a.Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(movie);
        }

        /*
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] CreateMovieDto dto)
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

            var index = _movies.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Movie not found", status = 404 });

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

        */

    }
        
}
