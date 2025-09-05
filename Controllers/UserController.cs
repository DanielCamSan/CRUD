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
        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(users);
        }

        // GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }
    }
}
