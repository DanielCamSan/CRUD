using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _animals = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Sebastian", Age = 19, Email= "sebas@gmail.com", Password= "lubevillas2008", },
            new User { Id = Guid.NewGuid(), Name = "Santiago", Age = 20, Email= "ghost@gmail.com", Password= "fercardenas", },
        };

        // READ: GET api/animals
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
            => Ok(_animals);

        // READ: GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            return animal is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(animal);
        }

        // CREATE: POST api/animals
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var animal = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Age = dto.Age,
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim()
            };

            _animals.Add(animal);
            return CreatedAtAction(nameof(GetOne), new { id = animal.Id }, animal);
        }

        // UPDATE (full): PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _animals.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = id,
                Name = dto.Name.Trim(),
                Age = dto.Age,
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim()
            };

            _animals[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _animals.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}