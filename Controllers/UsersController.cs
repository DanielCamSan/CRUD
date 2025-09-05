using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User {Id=Guid.NewGuid(), Name="Caleb", Age=21, email="caleb@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("Johnny234")))},
            new User {Id=Guid.NewGuid(), Name="Darius", Age=21, email="darius@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("@Darius36")))}
        };
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
            => Ok(_users);
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);

        }
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Age = dto.Age,
                email = dto.email,
                password = dto.password
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Age = dto.Age,
                email = dto.email,
                password = dto.password
            };

            _users[index] = updated;
            return Ok(updated);
        }
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