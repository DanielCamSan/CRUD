using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
            {
                new User { Id = Guid.NewGuid(), Name = "Carlos", Age = 52 },
                new User { Id = Guid.NewGuid(), Name = "Angel", Age = 21 }
            };

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_users);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] User user)
        {
            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id;
            _users[index] = user;
            return Ok(user);
        }

        [HttpPatch("{id:guid}")]
        public ActionResult<User> Patch(Guid id, [FromBody] User partial)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            if (user is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
            if (partial.Age > 0) user.Age = partial.Age;

            return Ok(user);
        }
    }
}
