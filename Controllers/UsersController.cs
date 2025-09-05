using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
            {
                new User { Id = Guid.NewGuid(), Name = "Lucas", Email = "lucas@example.com", Age = 22 },
                new User { Id = Guid.NewGuid(), Name = "Ana",   Email = "ana@example.com",   Age = 20 }
            };

        // GET api/animals
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
        {
            return Ok(_users);
        }

        // GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/animals
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] User user)
        {
            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id; // conservar el mismo Id
            _users[index] = user;
            return Ok(user);
        }

        // PATCH api/animals/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<User> Patch(Guid id, [FromBody] User partial)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            if (user is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
            if (!string.IsNullOrEmpty(partial.Email)) user.Email = partial.Email;
            if (partial.Age > 0) user.Age = partial.Age;

            return Ok(user);
        }

        // DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _users.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}