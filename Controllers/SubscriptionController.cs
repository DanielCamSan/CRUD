using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll() => Ok(_subscriptions);

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
