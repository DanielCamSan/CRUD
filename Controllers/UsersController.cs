using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _books = new()
            {
                new User { Id = Guid.NewGuid(), Name = "Luis", Email = "luis@gmail.com", Age = 30 },
                new User { Id = Guid.NewGuid(), Name = "Juan", Email = "juan@gmail.com", Age = 22 }
            };

        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_books);
        }

        // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _books.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _books.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] User user)
        {
            var index = _books.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id; // conservar el mismo Id
            _books[index] = user;
            return Ok(user);
        }

        // PATCH api/users/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<User> Patch(Guid id, [FromBody] User partial)
        {
            var user = _books.FirstOrDefault(a => a.Id == id);
            if (user is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
            if (!string.IsNullOrEmpty(partial.Email)) user.Email = partial.Email;
            if (partial.Age > 0) user.Age = partial.Age;

            return Ok(user);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _books.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
