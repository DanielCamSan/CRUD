using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> User = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Ana Torres",Email = "ana.torres@example.com",Passsword = "AnaSecure123",Age = 28},
            new User {Id = Guid.NewGuid(), Name = "Carlos Méndez",Email = "carlos.mendez@example.com", Passsword = "CarlosPass456!",Age = 35}
        };

        // READ: GET api/Users
        [HttpGet]

        public ActionResult<IEnumerable<User>> GetAll()
        => Ok(User);

        // READ: GET api/Users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var User = User.FirstOrDefault(a => a.Id == id);
            return User is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(User);
        }

        // CREATE: POST api/Users
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var User = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };
            User.Add(User);
            return CreatedAtAction(nameof(GetOne), new { id = User.Id }, User);
        }

        // UPDATE (full): PUT api/Users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = User.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = id,
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };

            User[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/Users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = User.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}