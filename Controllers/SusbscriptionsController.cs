using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription { Id = Guid.NewGuid(), Name = "Gold", subcription_date = "12/08/2025", duration = 1 },
            new Subscription { Id = Guid.NewGuid(), Name = "Premium", subcription_date = "21/10/2025", duration = 2 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()
            => Ok(_subscriptions);

        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(a => a.Id == id);
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
                subcription_date = dto.subcription_date,
                duration = dto.duration
            };

            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }

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
                duration = dto.duration
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }


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