using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> User = new()
        {
            new User { Id = Guid.NewGuid(), Name = "Ana Torres",Email = "ana.torres@example.com",Passsword = "AnaSecure123",Age = 28},
            new User {Id = Guid.NewGuid(), Name = "Carlos Méndez",Email = "carlos.mendez@example.com", Passsword = "CarlosPass456!",Age = 35}
        };

        private static (int pg, int lim) Normalizepg(int? pg, int? lim)
        {
            var p = pg.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = lim.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
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
        // READ: GET api/Users
        [HttpGet]

        public ActionResult<IEnumerable<User>> GetAll()
        => Ok(User);
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? pg,
            [FromQuery] int? lim,
            [FromQuery] string? sort,      
            [FromQuery] string? order,    
            [FromQuery] string? q,        
        )
        {
            var (p, l) = Normalizepg(pg, lim);

            IEnumerable<User> query = users;
            //free search
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            //does not require a general search by category
            //dinamic order
            query = OrderByProp(query, sort, order);

            //pagination
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { pg = p, lim = l, total }
            });
        }


        // READ: GET api/Users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<User> GetOne(Guid id)
        {
            var User = User.FirstOrDefault(a => a.Id == id);
            return User is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(User);
        }

        // CREATE: POST api/Users
        [HttpPost]
        public ActionResult<User> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var User = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Password = dto.Password.Trim(),
                Age = dto.Age
            };
            User.Add(User);
            return CreatedAtAction(nameof(GetOne), new { id = User.Id }, User);
        }

        // UPDATE (full): PUT api/Users/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<User> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = User.FindIndex(a => a.Id == id);
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

            User[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/Users/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = User.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }
}