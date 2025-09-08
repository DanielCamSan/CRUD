using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        // Datos en memoria (demo)
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Basic",   SubscriptionDate = DateTime.UtcNow.AddDays(-45), Duration = 30 },
            new Subscription { Id = Guid.NewGuid(), Name = "Premium", SubscriptionDate = DateTime.UtcNow.AddDays(-20), Duration = 90 },
            new Subscription { Id = Guid.NewGuid(), Name = "Gold",    SubscriptionDate = DateTime.UtcNow.AddDays(-10), Duration = 365 },
            new Subscription { Id = Guid.NewGuid(), Name = "Trial",   SubscriptionDate = DateTime.UtcNow.AddDays(-5),  Duration = 7  },
        };

        // Helpers reutilizando tu estilo
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

        // ✅ LIST: GET api/subscriptions (paginación + ordenamiento + búsqueda + filtros)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,        // name | subscriptionDate | duration
            [FromQuery] string? order,       // asc | desc
            [FromQuery] string? q,           // búsqueda en Name (contains)
            [FromQuery] DateTime? dateFrom,  // filtro fecha inicio >=
            [FromQuery] DateTime? dateTo,    // filtro fecha fin   <=
            [FromQuery] int? minDuration,    // filtro duración mínima
            [FromQuery] int? maxDuration     // filtro duración máxima
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            // 🔎 búsqueda libre (Name)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s => s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 📅 filtros por rango de fechas
            if (dateFrom.HasValue) query = query.Where(s => s.SubscriptionDate >= dateFrom.Value);
            if (dateTo.HasValue) query = query.Where(s => s.SubscriptionDate <= dateTo.Value);

            // ⏱️ filtros por rango de duración
            if (minDuration.HasValue) query = query.Where(s => s.Duration >= minDuration.Value);
            if (maxDuration.HasValue) query = query.Where(s => s.Duration <= maxDuration.Value);

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

        // READ: GET api/subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var sub = _subscriptions.FirstOrDefault(s => s.Id == id);
            return sub is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(sub);
        }

        // CREATE: POST api/subscriptions
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] CreateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var sub = new Subscription
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration
            };

            _subscriptions.Add(sub);
            return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, sub);
        }

        // UPDATE: PUT api/subscriptions/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(s => s.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                Name = dto.Name.Trim(),
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/subscriptions/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _subscriptions.RemoveAll(s => s.Id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }

    // ─────────────────────────────────────────────────────────────
    // OPCIONAL: Si aún no tienes el modelo/DTOs, puedes usar esto:
    // (Si ya los tienes en Models/, borra esta sección.)
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime SubscriptionDate { get; set; }
        public int Duration { get; set; }
    }

    public class CreateSubscriptionDto
    {
        public string Name { get; set; } = "";
        public DateTime SubscriptionDate { get; set; }
        public int Duration { get; set; }
    }

    public class UpdateSubscriptionDto : CreateSubscriptionDto { }
    // ─────────────────────────────────────────────────────────────
}
