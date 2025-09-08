using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private static readonly List<Animal> _animals = new()
        {
            new Animal { Id = Guid.NewGuid(), Name = "Luna", Species = "Dog", Age = 3 },
            new Animal { Id = Guid.NewGuid(), Name = "Michi", Species = "Cat", Age = 2 },
            new Animal { Id = Guid.NewGuid(), Name = "Rocky", Species = "Dog", Age = 5 },
            new Animal { Id = Guid.NewGuid(), Name = "Nala", Species = "Cat", Age = 1 },
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

        // ✅ LIST: GET api/animals  (con paginación + ordenamiento + búsqueda + filtro)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: name | species | age
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,        // búsqueda en Name/Species (contains)
            [FromQuery] string? species   // filtro exacto por especie (Dog/Cat/...)
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Animal> query = _animals;

            // 🔎 búsqueda libre (Name/Species)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Species.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtro específico (Species)
            if (!string.IsNullOrWhiteSpace(species))
            {
                query = query.Where(a => a.Species.Equals(species, StringComparison.OrdinalIgnoreCase));
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

        // READ: GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Animal> GetOne(Guid id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            return animal is null
                ? NotFound(new { error = "Animal not found any", status = 404 })
                : Ok(animal);
        }

        // CREATE: POST api/animals
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] CreateAnimalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Age = dto.Age
            };

            _animals.Add(animal);
            return CreatedAtAction(nameof(GetOne), new { id = animal.Id }, animal);
        }

        // UPDATE: PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateAnimalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _animals.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Animal not found", status = 404 });

            var updated = new Animal
            {
                Id = id,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Age = dto.Age
            };

            _animals[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _animals.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Animal not found", status = 404 })
                : NoContent();
        }
    }
}
