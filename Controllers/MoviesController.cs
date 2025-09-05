
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController: ControllerBase
    {
        private static readonly List<Movies> _movies = new()
        {
            new Movies { Id = Guid.NewGuid(), Title = "Conjuro4", Gender = "Horror", Year = 2025 },
            new Movies { Id = Guid.NewGuid(), Title = "Bestia", Gender = "Romantic", Year = 2020 }
        };

        // READ: GET api/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movies>> GetAll()
            => Ok(_movies);

        // READ: GET api/movies/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Movies> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(a => a.Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(movie);
        }
        // CREATE: POST api/movies
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] CreateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var movie = new Movies
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Gender = dto.Gender.Trim(),
                Year = dto.Year
            };

            _movies.Add(movie);
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie);
        }

        


    }
}


   
       