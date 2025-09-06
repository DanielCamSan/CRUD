using System.Security.Cryptography;
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
            new User {Id=Guid.NewGuid(), Name="Caleb", Age=21, email="caleb@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("Johnny234")))},
            new User {Id=Guid.NewGuid(), Name="Darius", Age=21, email="darius@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("@Darius36")))},
            new User {Id=Guid.NewGuid(), Name="Tom", Age=25, email="tommy@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("$Tommyboy24")))},
            new User {Id=Guid.NewGuid(), Name="Scott", Age=41, email="mescudi@example.com", password=Convert.ToHexString(MD5.HashData(System.Text.Encoding.UTF8.GetBytes("$KidCudi36")))}

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

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
         public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,    
            [FromQuery] string? order,    
            [FromQuery] string? q,        
            [FromQuery] string? Name,
            [FromQuery] string? email,
            [FromQuery] string? Age

        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<User> query = _users;
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                a.Name.ToLower().Contains(q.ToLower()) ||
                a.Age.ToString().Contains(q) ||
                a.email.ToLower().Contains(q.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(a => a.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(a => a.email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(Age))
            {
                query = query.Where(a => a.Age.ToString() == Age);
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
            var user = _users.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);

        }
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Age = dto.Age,
                email = dto.email,
                password = dto.password
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _users.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new User
            {
                Id = Guid.NewGuid(),
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