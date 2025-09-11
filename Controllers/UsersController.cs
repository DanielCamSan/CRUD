using System.Reflection;
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
            new User { Id = Guid.NewGuid(), name = "Alex", age = 50, email = "alex@gmail.com", password = "123321" },
            new User { Id = Guid.NewGuid(), name = "Pedro", age = 25, email = "pedro@gmail.com", password = "111222333" },
            new User { Id = Guid.NewGuid(), name = "Maria", age = 30, email = "maria@hotmail.com", password = "maria2025" },
            new User { Id = Guid.NewGuid(), name = "Luis", age = 40, email = "luis@yahoo.com", password = "luisPass99" },
            new User { Id = Guid.NewGuid(), name = "Carla", age = 22, email = "carla@gmail.com", password = "carla123" },
            new User { Id = Guid.NewGuid(), name = "Andres", age = 35, email = "andres@hotmail.com", password = "andresSecure" },
            new User { Id = Guid.NewGuid(), name = "Sofia", age = 28, email = "sofia@yahoo.com", password = "sofia456" },
            new User { Id = Guid.NewGuid(), name = "Jorge", age = 45, email = "jorge@gmail.com", password = "jorgePass" },
            new User { Id = Guid.NewGuid(), name = "Lucia", age = 19, email = "lucia@hotmail.com", password = "lucia789" },
            new User { Id = Guid.NewGuid(), name = "Fernando", age = 32, email = "fernando@yahoo.com", password = "fer321" }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)? src.OrderByDescending(x => prop.GetValue(x)) : src.OrderBy(x => prop.GetValue(x));
        }


        [HttpGet]
        public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? limit, [FromQuery] string? sort, [FromQuery] string? order, [FromQuery] string? q )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<User> query = users;


            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>a.name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);


            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

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
