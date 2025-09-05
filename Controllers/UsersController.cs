using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController :ControllerBase
    {
        private static readonly List<User> users = new()
        {
            new User { Id = Guid.NewGuid(), name = "Alex", age =50, email = "alex@gmail.com", password="123321" },
            new User { Id = Guid.NewGuid(), name = "Pedro", age =25, email = "pedro@gmail.com", password="111222333" }
        };


        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll() => Ok(users);

        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = users.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);
        }

        [HttpPost]
        public ActionResult<Animal> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new User
            {
                Id = Guid.NewGuid(),
                name = dto.name.Trim(),
                age = dto.age,
                email= dto.email.Trim(),
                password= dto.password.Trim()
            };

            users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = users.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = id,
                name = dto.name.Trim(),
                age = dto.age,
                email = dto.email.Trim(),
                password = dto.password.Trim()
            };

            users[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = users.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}
