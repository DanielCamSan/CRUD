using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Carlos Pérez", UserType = "Estudiante" },
            new User { Id = Guid.NewGuid(), Name = "Ana López", UserType = "Docente" }
        };

        // GET api/users
        [HttpGet]
        public IEnumerable<User> GetAll() => _users;

        // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, User user)
        {
            var index = _users.FindIndex(u => u.Id == id);
            if (index == -1) return NotFound();

            user.Id = id;
            _users[index] = user;
            return Ok(user);
        }

        // PATCH api/users/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<User> Patch(Guid id, User partial)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
            if (!string.IsNullOrEmpty(partial.UserType)) user.UserType = partial.UserType;

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
