using Microsoft.AspNetCore.Mvc;
using static CreateUserDto;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
    {
       
        
        public class UsersController : ControllerBase
        {
            private static readonly List<User> _Users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Luna", Email = "LunaIsHere.@gmail.com",Password = "MyBirthday", Age = 3 },
            new User { Id = Guid.NewGuid(), Name = "Mishel",Email = "Mishigato123.@gmail.com",Password = "Password1234", Age = 2 }
        };

            // READ: GET api/animals
            [HttpGet]
            public ActionResult<IEnumerable<User>> GetAll()
                => Ok(_Users);

            // READ: GET api/animals/{id}
            [HttpGet("{id:guid}")]
            public ActionResult<User> GetOne(Guid id)
            {
                var user = _Users.FirstOrDefault(a => a.Id == id);
                return user is null
                    ? NotFound(new { error = "user not found", status = 404 })
                    : Ok(user);
            }

            // CREATE: POST api/animals
            [HttpPost]
            public ActionResult<User> Create([FromBody] CreateUserDto dto)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name.Trim(),
                    Password = dto.Password.Trim(),
                    Email = dto.Email.Trim(),
                    Age = dto.Age
                };

                _Users.Add(user);
                return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
            }

            // UPDATE (full): PUT api/animals/{id}
            [HttpPut("{id:guid}")]
            public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var index = _Users.FindIndex(a => a.Id == id);
                if (index == -1)
                    return NotFound(new { error = "User not found", status = 404 });

                var updated = new User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name.Trim(),
                    Password = dto.Password.Trim(),
                    Email = dto.Email.Trim(),
                    Age = dto.Age
                };

                _Users[index] = updated;
                return Ok(updated);
            }

            // DELETE: DELETE api/animals/{id}
            [HttpDelete("{id:guid}")]
            public IActionResult Delete(Guid id)
            {
                var removed = _Users.RemoveAll(a => a.Id == id);
                return removed == 0
                    ? NotFound(new { error = "User not found", status = 404 })
                    : NoContent();
            }
        }
    }
}

