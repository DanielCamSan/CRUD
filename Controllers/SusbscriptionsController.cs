using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Gold", subcription_date = "12/08/2025", Duration = 1 },
            new Subscription { Id = Guid.NewGuid(), Name = "Premium", subcription_date = "21/10/2025", Duration = 2 },
            new Subscription { Id = Guid.NewGuid(), Name = "Diamond", subcription_date = "12/08/2025", Duration = 8 },
            new Subscription { Id = Guid.NewGuid(), Name = "SuperPremium", subcription_date = "21/10/2025", Duration = 6 }
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

        // LIST: GET api/subscriptions (con paginación + ordenamiento + búsqueda + filtro)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: Name | duration
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,        
            [FromQuery] string? name,
            [FromQuery] string? subscrptionDate, 
            [FromQuery] int? duration         
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            // busqueda libre name/date
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s =>
                    s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    s.subcription_date.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // filtro específico (Name)
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s=> s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            // filtro especifico (Duration)
            if (duration.HasValue)
            {
                query = query.Where(s => s.Duration == duration.Value);
            }

            // ordenamiento dinamico (safe)
            query = OrderByProp(query, sort, order);

            // paginacion
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // READ: GET api/Subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(a => a.Id == id);
            return subscription is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(subscription);
        }

        // CREATE: POST api/Subscriptions
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] CreateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                subcription_date = dto.subcription_date,
                Duration = dto.Duration
            };

            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }


        // UPDATE: PUT api/subscriptions/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                Name = dto.Name.Trim(),
                subcription_date = dto.subcription_date,
                Duration = dto.Duration
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/subscriptions/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _subscriptions.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }
}