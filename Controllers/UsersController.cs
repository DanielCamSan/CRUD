using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class UsersController : ControllerBase
    {
        private static readonly List<User> users = new()
        {
            new User { Id = Guid.NewGuid(),Name="Pedro"},
            new User { Id = Guid.NewGuid(),Name="Juan"}
        };
        [HttpGet]
        public ActionResult<List<User>> readUsers()
        {
            return users;
        }
        [HttpGet("{id:guid}")]
        public ActionResult<User> readOneUser(Guid id)
        {
            var userId = users.FirstOrDefault(a => a.Id == id);
            return userId is null ? NotFound() : userId;
        }
        [HttpPost]
        public ActionResult<User> createUser([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            users.Add(user);
            return CreatedAtAction(nameof(readOneUser), new { id = user.Id }, user);
        }
        [HttpPatch("{id:guid}")]
        public ActionResult<User> updatePatch(Guid id, [FromBody] User partial)
        {
            var user = users.FirstOrDefault(p => p.Id == id);
            if (user is null) return NotFound();
            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
            if (partial.age > 0) user.age = partial.age;

            return Ok(user);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] User user)
        {
            var index = users.FindIndex(p => p.Id == id);
            if (index == -1) return NotFound();

            user.Id = id;
            users[index] = user;
            return Ok(user);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = users.RemoveAll(p => p.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
