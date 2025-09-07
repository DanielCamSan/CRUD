using System.Reflection;
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
            new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@mail.com", Password = "Passw0rd!", Age = 15 },
            new User { Id = Guid.NewGuid(), Name = "Charlie", Email = "charlie@mail.com", Password = "Charlie123", Age = 22 },
            new User { Id = Guid.NewGuid(), Name = "Diana", Email = "diana@mail.com", Password = "DianaPass", Age = 50 }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no ordenar
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }
        // READ: GET api/users
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,   // ejemplo: name | email | age
            [FromQuery] string? order,  // asc | desc
            [FromQuery] string? q,      // búsqueda en Name/Email
            [FromQuery] int? minAge,    // filtro extra: edad mínima
            [FromQuery] int? maxAge     // filtro extra: edad máxima
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<User> query = _users;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u =>
                    u.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (minAge.HasValue)
                query = query.Where(u => u.Age >= minAge.Value);

            if (maxAge.HasValue)
                query = query.Where(u => u.Age <= maxAge.Value);

            query = OrderByProp(query, sort, order);

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

        // UPDATE (full): PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(u => u.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = id,
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };

            _users[index] = updated;
            return Ok(updated);
        }
        // DELETE: DELETE api/users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _users.RemoveAll(u => u.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}