using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> subscriptions = new()
        {
            new Subscription {Id = Guid.NewGuid(), Name = "Andy", Duration = 365, SubscriptionDate = DateOnly.FromDateTime(DateTime.Now) },
            new Subscription {Id = Guid.NewGuid(), Name = "Dabner", Duration = 365, SubscriptionDate = DateOnly.FromDateTime(DateTime.Now) }
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
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,    
            [FromQuery] string? order,   
            [FromQuery] string? q      
        )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Subscription> query = subscriptions;

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(s => s.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

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
            var sub = subscriptions.FirstOrDefault(s => s.Id == id);
            return sub is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(sub);
        }

        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] Subscription.CreateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var sub = new Subscription
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Duration = dto.Duration,
                SubscriptionDate = DateOnly.FromDateTime(DateTime.Now)
            };

            subscriptions.Add(sub);
            return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, sub);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] Subscription.UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = subscriptions.FindIndex(s => s.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                Name = dto.Name.Trim(),
                Duration = dto.Duration,
                SubscriptionDate = subscriptions[index].SubscriptionDate
            };

            subscriptions[index] = updated;
            return Ok(updated);
        }


    }
}
