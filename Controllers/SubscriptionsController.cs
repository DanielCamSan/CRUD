using Microsoft.AspNetCore.Mvc;

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
            }
        };

        // GET: api/subscriptions
        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()
            => Ok(_subscriptions);

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
