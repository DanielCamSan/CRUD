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
        public ActionResult<IEnumerable<Subscription>> GetAll()=> Ok(_subscriptions);

        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(a => a.id == id);
            return subscription is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(subscription);
        }

    }
}
