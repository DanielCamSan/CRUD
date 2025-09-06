using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriptionDate = DateTime.UtcNow.AddDays(-7),
                Duration = 30,
                Name = "Premium Monthly"
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriptionDate = DateTime.UtcNow.AddDays(-15),
                Duration = 365,
                Name = "Annual Plan"
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriptionDate = DateTime.UtcNow.AddDays(-3),
                Duration = 7,
                Name = "Weekly Trial"
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriptionDate = DateTime.UtcNow.AddDays(-30),
                Duration = 90,
                Name = "Quarterly Basic"
            }
        };

        // ✅ Normalizar parámetros de paginación
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;

            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if (l > 100) l = 100;

            return (p, l);
        }

        // ✅ Ordenamiento dinámico por propiedad
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;

            var prop = typeof(T).GetProperty(sort,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        // ✅ GET ALL con paginación, ordenamiento, búsqueda y filtros
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     
            [FromQuery] string? order,   
            [FromQuery] string? q,        
            [FromQuery] int? minDuration, 
            [FromQuery] int? maxDuration, 
            [FromQuery] DateTime? startDate, 
            [FromQuery] DateTime? endDate   
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s =>
                    s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (minDuration.HasValue)
            {
                query = query.Where(s => s.Duration >= minDuration.Value);
            }

            if (maxDuration.HasValue)
            {
                query = query.Where(s => s.Duration <= maxDuration.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(s => s.SubscriptionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.SubscriptionDate <= endDate.Value);
            }

            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new
                {
                    page = p,
                    limit = l,
                    total,
                    totalPages = (int)Math.Ceiling(total / (double)l),
                    hasNextPage = p < Math.Ceiling(total / (double)l),
                    hasPreviousPage = p > 1
                }
            });
        }

        // GET: api/subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == id);
            return subscription is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(subscription);
        }

        // POST: api/subscriptions
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

            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }

        // PUT: api/subscriptions/{id}
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
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration,
                Name = dto.Name.Trim()
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        // DELETE: api/subscriptions/{id}
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
