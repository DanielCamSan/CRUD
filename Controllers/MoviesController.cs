using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

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


        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

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
            [FromQuery] string? name)
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Movie> query = _movies;

            // Filtro por búsqueda general (q) o por nombre
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(m => m.Title.Contains(q, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Title.Contains(name, StringComparison.OrdinalIgnoreCase));

            // Ordenamiento
            query = OrderByProp(query, sort, order);

            // Paginación
            var total = query.Count();
            var items = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new { total, page = p, limit = l, items });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Movie> GetOne(Guid id)
        {
            var movie = _movies.FirstOrDefault(a => a.Id == id);
            return movie is null
                ? NotFound(new { error = "Movie not found", status = 404 })
                : Ok(movie);
        }


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

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _movies.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Movie not found", status = 404 })
                : NoContent();

        }

    }
}