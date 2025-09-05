using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private static readonly List<User> users = new()
            {
                new User { Id = Guid.NewGuid(), Username = "dahbner", Age = 19},
                new User { Id = Guid.NewGuid(), Username = "andy.andrade", Age = 20}
            };
        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] User user)
        {
            var index = users.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id;
            users[index] = user; //si
            return Ok(user);
        }

        // PATCH api/users/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Animal> Patch(Guid id, [FromBody] User partial)
        {
            var user = users.FirstOrDefault(a => a.Id == id);
            if (user is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.Username)) user.Username = partial.Username;
            if (partial.Age > 0) user.Age = partial.Age;

            return Ok(user);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = users.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }

    }
}
