using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Globalization;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private static readonly List<Users> _users = new()
        {
            new Users { Id = Guid.NewGuid(), Name = "Carlos", Age = 25, email = "carlitos@gmail.com",password="123carlos"},
            new Users { Id = Guid.NewGuid(), Name = "Mateo", Age = 27, email = "mat@gamil.com",password="mati345" },
            new Users { Id = Guid.NewGuid(), Name = "Sandra", Age = 30, email = "san@gmail.com",password="sandrita123"},
            new Users { Id = Guid.NewGuid(), Name = "Rosa", Age = 20, email = "ros@gamil.com",password="rosi555" }
        };
        
         private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        //ordenamiento dinamico
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: name | species | age
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,       // búsqueda por nombre o email
            [FromQuery] int? minAge,     // edad mínima
            [FromQuery] int? maxAge      // edad máxima

        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Users> query = _users;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.email.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            // 🎯 Filtro por edad mínima
            if (minAge.HasValue)
            {
                query = query.Where(a => a.Age >= minAge.Value);
            }

            // 🎯 Filtro por edad máxima
            if (maxAge.HasValue)
            {
                query = query.Where(a => a.Age <= maxAge.Value);
            }

            // ↕️ ordenamiento dinámico (safe)
            query = OrderByProp(query, sort, order);

            // 📄 paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Users> GetOne(Guid id)
        {
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);
        }
        
        [HttpPost]
        public ActionResult<Users> Create([FromBody] CreateUsersDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new Users
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Age = dto.Age,
                email=dto.email,
                password=dto.password
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }
        
        [HttpPut("{id:guid}")]
        public ActionResult<Users> Update(Guid id, [FromBody] UpdateUsersDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new Users
            {
                Id = id,
                Name = dto.Name.Trim(),
                Age = dto.Age,
                email = dto.email,
                password = dto.password
            };

            _users[index] = updated;
            return Ok(updated);
        }
     
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
