using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new();
        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()
        {
            return Ok(_subscriptions);
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == id);

            return subscription is null ? NotFound() : Ok(subscription);
        }
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] Subscription subscription)
        {
            subscription.Id = Guid.NewGuid();
            _subscriptions.Add(subscription);
            return CreatedAtAction(nameof(GetOne), new { id = subscription.Id }, subscription);
        }
    }
   }
