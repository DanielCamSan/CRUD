using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> subscriptions = new()
        {
            new Subscription {Id = Guid.NewGuid(), Name = "Andy", Duration = 365, SubscriptionDate = DateOnly.FromDateTime(DateTime.Now) },
            new Subscription {Id = Guid.NewGuid(), Name = "Dabner", Duration = 365, SubscriptionDate = DateOnly.FromDateTime(DateTime.Now) }
        };


    }
}
