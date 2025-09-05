using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Juan Pérez", Email = "juan@example.com", Phone = "76543210" },
            new User { Id = Guid.NewGuid(), Name = "María López", Email = "maria@example.com", Phone = "71234567" }
        };

        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_users);
        }

        // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] User user)
        {
            var index = _users.FindIndex(u => u.Id == id);
            if (index == -1) return NotFound();

            user.Id = id; // conservar el mismo Id
            _users[index] = user;
            return Ok(user);
        }


        // DELETE api/users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _users.RemoveAll(u => u.Id == id);
            return removed == 0 ? NotFound() : NoContent();

        }
    }
}