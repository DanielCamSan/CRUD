using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace newCRUD.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new() {
           new Subscription {id=Guid.NewGuid(), name = "Rodrigo", date = DateTime.Now, duracion=12},
           new Subscription {id=Guid.NewGuid(), name = "Luis", date = DateTime.Now, duracion=15},
           new Subscription {id=Guid.NewGuid(), name = "Randall", date = DateTime.Now, duracion=30},
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
            [FromQuery] string? q,       
            [FromQuery] string? name   
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Subscription> query = _subscriptions;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();

                var esNumero = int.TryParse(term, out var dur);
                var esFecha = DateTime.TryParse(term, out var dt);

                query = query.Where(a =>
                    a.name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (esNumero && a.duracion == dur) ||
                    (esFecha && a.date.Date == dt.Date)   
                );
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
            var subscription = _subscriptions.FirstOrDefault(a => a.id == id);
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
                id = Guid.NewGuid(),
                name = dto.name.Trim(),
                date = dto.date,
                duracion = dto.duracion,
            };

            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.id }, subscription);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] UpdateSubscription dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(a => a.id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Subscription
            {
                id = Guid.NewGuid(),
                name = dto.name.Trim(),
                date = dto.date,
                duracion = dto.duracion,
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _subscriptions.RemoveAll(a => a.id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }
}
