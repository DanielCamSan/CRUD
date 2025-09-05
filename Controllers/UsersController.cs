using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@mail.com", Password = "Secret123", Age = 25 },
            new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@mail.com", Password = "Passw0rd!", Age = 30 }
        };

        // READ: GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
            => Ok(_users);

        // READ: GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);
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
    }
}