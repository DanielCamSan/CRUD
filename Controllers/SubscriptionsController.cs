using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> subs = new()
        {
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 9, 1), Duration = TimeSpan.FromDays(30), Name = "Basic Plan" },
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 8, 15), Duration = TimeSpan.FromDays(90), Name = "Premium"},
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 7, 10), Duration = TimeSpan.FromDays(7), Name = "Trial" },
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 9, 5), Duration = TimeSpan.FromDays(365), Name = "Annual" },
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
        [FromQuery] string? sort,     // ejemplo: name | subscriptionDate | duration
        [FromQuery] string? order,    // asc | desc
        [FromQuery] string? q,        // búsqueda en Name (contains)
        [FromQuery] TimeSpan? minDuration // filtro por duración mínima
    )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = subs;

            // búsqueda libre (Name)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s =>
                    s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // filtro específico (Duración mínima)
            if (minDuration.HasValue)
            {
                query = query.Where(s => s.Duration >= minDuration.Value);
            }

            // ordenamiento dinámico
            query = OrderByProp(query, sort, order);

            // paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }


        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = subs.FirstOrDefault(a => a.Id == id);
            return subscription is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(subscription);
        }
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] CreateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration,
                Name = dto.Name.Trim()
            };

            subs.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = subs.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });
            var updated = new Subscription
            {
                Id = id,
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration,
                Name = dto.Name.Trim()
            };
            subs[index] = updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = subs.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }
}