using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Luis Linares", Age = 25, Email = "luislinares@gmil.com", Password = "qwerty" },
            new User { Id = Guid.NewGuid(), Name = "Erick Camacho", Age = 30, Email = "erickcamacho@gmail.com", Password = "asdfgh" }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;

            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if (l > 100) l = 100;

            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        // LIST: GET api/users  
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q,
            [FromQuery] int? age
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<User> query = _users;

            // (Name o Email)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // f(Age)
            if (age.HasValue)
            {
                query = query.Where(a => a.Age == age.Value);
            }

            // order
            query = OrderByProp(query, sort, order);

            // pag
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // READ: GET api/users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
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
                Age = dto.Age,
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim()
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // UPDATE (full): PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(a => a.Id == id);
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

            _users[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/users/{id}
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
