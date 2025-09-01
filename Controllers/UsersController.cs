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

        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }
    }
}
