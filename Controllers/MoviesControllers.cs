using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

        // 🔹 Helpers
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

        // ✅ LIST: GET api/v1/movies (con paginación + ordenamiento + búsqueda + filtro)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: Title | Year | Genre
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,        // búsqueda en Title/Genre (contains)
            [FromQuery] string? genre,    // filtro exacto por género
            [FromQuery] int? yearFrom,    // año mínimo
            [FromQuery] int? yearTo       // año máximo
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Movie> query = _movies;

            // 🔎 búsqueda libre (Title/Genre)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(m =>
                    m.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    m.Genre.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtro específico (Genre)
            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            }

            // 📅 filtros por rango de año
            if (yearFrom.HasValue) query = query.Where(m => m.Year >= yearFrom.Value);
            if (yearTo.HasValue) query = query.Where(m => m.Year <= yearTo.Value);

            // ↕️ ordenamiento dinámico
            query = OrderByProp(query, sort, order);

            // 📄 paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

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
