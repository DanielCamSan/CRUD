using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
            {
                new User { Id = Guid.NewGuid(), Name = "Carlos", Age = 52 },
                new User { Id = Guid.NewGuid(), Name = "Angel", Age = 21 }
            };

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_users);
        }
    }
}
