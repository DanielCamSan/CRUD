using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Luna", email = "luna@gmail.com", Age = 3, Password = "12345" },
            new User { Id = Guid.NewGuid(), Name = "Paquita", Species = "paquita@gmail.com", Age = 2, Password = "12345" }
        };

        // READ: GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
            => Ok(_users);

        // READ: GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "Animal not found", status = 404 })
                : Ok(animal);
        }

        // CREATE: POST api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // UPDATE (full): PUT api/users/{id}
        // UPDATE (full): PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = id,
                Name = dto.Name.Trim(),
                Email = dto.EmailAddress.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };

            _users[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _users.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}