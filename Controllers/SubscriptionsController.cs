using Microsoft.AspNetCore.Mvc;
using System.Reflection; 

namespace SubscriptionsCRUD.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
            {
                new Subscription { Id = Guid.NewGuid(), name = "Lucia", subscription_date = new DateOnly(2025,08,4), duration = 30},
                new Subscription { Id = Guid.NewGuid(), name = "Marcelo", subscription_date = new DateOnly(2025,09,12), duration = 30},
                new Subscription { Id = Guid.NewGuid(), name = "Daniel", subscription_date = new DateOnly(2025,01,11), duration = 60},
                new Subscription { Id = Guid.NewGuid(), name = "Martin", subscription_date = new DateOnly(2020,07,3), duration = 60},
                new Subscription { Id = Guid.NewGuid(), name = "Bruno", subscription_date = new DateOnly(2027,12,24), duration = 30}
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

        // ✅ LIST: GET api/subscriptions  (con paginación + ordenamiento + búsqueda + filtro)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: name | species | age
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q        // búsqueda en Name/Species (contains)
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            // 🔎 búsqueda libre (Name/Species)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.name.Contains(q, StringComparison.OrdinalIgnoreCase));
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

        // GET api/subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Animal> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(a => a.Id == id);
            return subscription is null ? NotFound(new { error = "Subscription not found", status = 404 }) : Ok(subscription);
        }
        // POST api/subscriptions
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] createSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var subscrip = new Subscription
            {
                Id = Guid.NewGuid(),
                name = dto.name.Trim(),
                subscription_date = new DateOnly(dto.subscription_date.Year, dto.subscription_date.Month, dto.subscription_date.Day),
                duration = dto.duration
            };
            return CreatedAtAction(nameof(GetOne), new { id = subscrip.Id }, subscrip);
        }

        // PUT api/subscription/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] updateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subcription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                name = dto.new_name,
                subscription_date = new DateOnly(dto.new_subscription_date.Year, dto.new_subscription_date.Month, dto.new_subscription_date.Day),
                duration = dto.new_duration
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        // DELETE api/subcriptions/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _subscriptions.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound(new { error = "Subcription not found", status = 404 }) : NoContent();
        }
        
    }
}
