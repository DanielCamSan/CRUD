
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

        // ✅ LIST: GET api/movies  (con paginación + ordenamiento + búsqueda + filtro)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: name | species | age
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,        // búsqueda en Name/Species (contains)
            [FromQuery] string? gender   // filtro exacto por especie (Dog/Cat/...)
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Movies> query = _movies;

            // 🔎 búsqueda libre (Title/Gender)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Gender.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtro específico (Gender)
            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(a => a.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
            }

            // ↕️ ordenamiento dinámico (safe)
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

        // UPDATE (full): PUT api/movies/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateMovieDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _movies.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Movie not found", status = 404 });

            var updated = new Movies
            {
                Id = id,
                Title = dto.Title.Trim(),
                Gender = dto.Gender.Trim(),
                Year = dto.Year
            };

            _movies[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/movies/{id}
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


   
       