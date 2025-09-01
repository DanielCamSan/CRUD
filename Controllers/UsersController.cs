using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private static readonly List<Users> _users = new()
        {
                new Users { Id = Guid.NewGuid(), Name = "Julia" },
                new Users { Id = Guid.NewGuid(), Name = "Matias" }
        };
        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAll()
        {
            return Ok(_users);
        }
       /* // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Users> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }
       */

    }
}
