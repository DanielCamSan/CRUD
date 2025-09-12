using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }


        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q,
            [FromQuery] string? genre
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Movie> query = movies;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Genre.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(a => a.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = movies.FirstOrDefault(a => a. Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found any", status = 404 })
                : Ok(movie);
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

            movies.Add(movie);
            return CreatedAtAction(nameof(GetOne), new { id = movie.Id }, movie);
        }

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
