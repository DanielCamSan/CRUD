using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> subs = new()
        {
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 9, 1), Duration = TimeSpan.FromDays(30), Name = "Basic Plan" },
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 8, 15), Duration = TimeSpan.FromDays(90), Name = "Premium"},
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 7, 10), Duration = TimeSpan.FromDays(7), Name = "Trial" },
            new Subscription { Id = Guid.NewGuid(), SubscriptionDate = new DateTime(2025, 9, 5), Duration = TimeSpan.FromDays(365), Name = "Annual" },
        };

        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()
        {
            return Ok(subs);
        }


        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = subs.FirstOrDefault(a => a.Id == id);
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
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration,
                Name = dto.Name.Trim()
            };

            subs.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }


        // UPDATE (full): PUT api/subscriptions/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = subs.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });
            var updated = new Subscription
            {
                Id = id,
                SubscriptionDate = dto.SubscriptionDate,
                Duration = dto.Duration,
                Name = dto.Name.Trim()
            };
            subs[index] = updated;
            return Ok(updated);
        }


        // DELETE: DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = subs.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }
}