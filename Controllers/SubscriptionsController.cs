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
    }
}
