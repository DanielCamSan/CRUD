using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Basic",   Duration = 1,  SubscriptionDate = DateTime.UtcNow.Date },
            new Subscription { Id = Guid.NewGuid(), Name = "Premium", Duration = 12, SubscriptionDate = DateTime.UtcNow.Date.AddDays(-7) }
        };

        // READ: GET api/subscriptions
        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()
            => Ok(_subscriptions);

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

            var entity = new Subscription
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Duration = dto.Duration,
                SubscriptionDate = dto.SubscriptionDate
            };

            _subscriptions.Add(entity);
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
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
                Duration = dto.Duration,
                SubscriptionDate = dto.SubscriptionDate
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
