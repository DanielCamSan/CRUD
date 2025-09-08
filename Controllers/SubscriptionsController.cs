using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using newCRUD.Models;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Netflix Premium", InDate = DateTime.Now.AddDays(-10), Duration = 30 },
            new Subscription { Id = Guid.NewGuid(), Name = "Spotify Familiar", InDate = DateTime.Now.AddDays(-5), Duration = 30 },
            new Subscription { Id = Guid.NewGuid(), Name = "HBO Max", InDate = DateTime.Now.AddDays(-20), Duration = 90 },
            new Subscription { Id = Guid.NewGuid(), Name = "Disney Plus Anual", InDate = DateTime.Now.AddDays(-100), Duration = 365 },
            new Subscription { Id = Guid.NewGuid(), Name = "Amazon Prime Video", InDate = DateTime.Now.AddDays(-3), Duration = 30 }
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

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,  // name | inDate | duration
            [FromQuery] string? order, // asc | desc
            [FromQuery] string? q      // Búsqueda en el campo 'Name'
        )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Subscription> query = _subscriptions;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(s => s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);

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
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == id);
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
                Name = dto.Name.Trim(),
                InDate = dto.InDate,
                Duration = dto.Duration
            };
            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }

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
                InDate = dto.InDate,
                Duration = dto.Duration
            };
            _subscriptions[index] = updated;
            return Ok(updated);
        }

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