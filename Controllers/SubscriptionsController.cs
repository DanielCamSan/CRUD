using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> /api/subscriptions
    public class SubscriptionsController : ControllerBase
    {
        // “DB” en memoria para demo
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Basic",   SubscriptionDate = DateTime.UtcNow.Date.AddDays(-7),  DurationMonths = 1  },
            new Subscription { Id = Guid.NewGuid(), Name = "Premium", SubscriptionDate = DateTime.UtcNow.Date.AddDays(-30), DurationMonths = 12 },
            new Subscription { Id = Guid.NewGuid(), Name = "Gold",    SubscriptionDate = DateTime.UtcNow.Date,              DurationMonths = 6  },
            new Subscription { Id = Guid.NewGuid(), Name = "Silver",  SubscriptionDate = DateTime.UtcNow.Date.AddDays(-2),  DurationMonths = 3  },
        };

        // ---- helpers como en el ejemplo ----
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

        // ✅ LIST: GET api/subscriptions (paginación + orden + búsqueda + filtros)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,        // name | subscriptionDate | durationMonths
            [FromQuery] string? order,       // asc | desc
            [FromQuery] string? q,           // búsqueda en Name (contains)
            [FromQuery] DateTime? dateFrom,  // filtro: fecha desde (inclusive)
            [FromQuery] DateTime? dateTo,    // filtro: fecha hasta (inclusive)
            [FromQuery] int? minDuration,    // filtro: duración mínima en meses
            [FromQuery] int? maxDuration     // filtro: duración máxima en meses
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            // 🔎 búsqueda libre por Name
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s => s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtros específicos
            if (dateFrom.HasValue)
                query = query.Where(s => s.SubscriptionDate.Date >= dateFrom.Value.Date);

            if (dateTo.HasValue)
                query = query.Where(s => s.SubscriptionDate.Date <= dateTo.Value.Date);

            if (minDuration.HasValue)
                query = query.Where(s => s.DurationMonths >= minDuration.Value);

            if (maxDuration.HasValue)
                query = query.Where(s => s.DurationMonths <= maxDuration.Value);

            // ↕ ordenamiento dinámico (seguro)
            query = OrderByProp(query, sort, order);

            // 📄 paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            // estructura de respuesta (simple, sin PagedResponse)
            return Ok(new
            {
                data,
                meta = new
                {
                    page = p,
                    limit = l,
                    total,
                    sort = sort,
                    order = order,
                    q,
                    dateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                    dateTo = dateTo?.ToString("yyyy-MM-dd"),
                    minDuration,
                    maxDuration
                }
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
                DurationMonths = dto.DurationMonths
            };

            _subscriptions.Add(sub);
            return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, sub);
        }

        // UPDATE (full): PUT api/subscriptions/{id}
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
                DurationMonths = dto.DurationMonths
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
}
