using Microsoft.AspNetCore.Mvc;

namespace SubscriptionsCRUD.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
            {
                new Subscription { Id = Guid.NewGuid(), name = "Lucia", subscription_date = new DateOnly(2025,08,4), duration = 30},
                new Subscription { Id = Guid.NewGuid(), name = "Marcelo", subscription_date = new DateOnly(2025,09,4), duration = 30}
            };

        // GET api/subscriptions
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
        {
            return Ok(_subscriptions);
        }

        // GET api/subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Animal> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(a => a.Id == id);
            return subscription is null ? NotFound(new { error = "Subscription not found", status = 404 }) : Ok(subscription);
        }
        // POST api/subscriptions
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] createSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var subscrip = new Subscription
            {
                Id = Guid.NewGuid(),
                name = dto.name.Trim(),
                subscription_date = new DateOnly(dto.subscription_date.Year, dto.subscription_date.Month, dto.subscription_date.Day), 
                duration = dto.duration
            };
            return CreatedAtAction(nameof(GetOne), new { id = subscrip.Id }, subscrip);
        }

        // PUT api/subscription/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] updateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subcription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                subscription_date = new DateOnly(dto.new_subscription_date.Year, dto.new_subscription_date.Month, dto.new_subscription_date.Day), 
                duration = dto.new_duration
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }
        
    }
}
