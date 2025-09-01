using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private static readonly List<Users> _users = new()
        {
                new Users { Id = Guid.NewGuid(), Name = "Julia" },
                new Users { Id = Guid.NewGuid(), Name = "Matias" }
        };
        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAll()
        {
            return Ok(_users);
        }
       // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Users> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }
        // POST api/users
        [HttpPost]
        public ActionResult<Users> Create([FromBody] Users user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }
        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Users> Update(Guid id, [FromBody] Users user)
        {
            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id; // conservar el mismo Id
            _users[index] = user;
            return Ok(user);
        }

        // PATCH api/animals/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Users> Patch(Guid id, [FromBody] Users partial)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            if (user is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) user.Name = partial.Name;
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
