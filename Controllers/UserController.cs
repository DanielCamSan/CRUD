using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private static readonly List<User> users = new()
            {
                new User { Id = Guid.NewGuid(), Username = "dahbner", Age = 19 },
                new User { Id = Guid.NewGuid(), Username = "andy.andrade", Age = 20 }
            };
    }
}
