using Microsoft.AspNetCore.Mvc;
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
            new Users { Id = Guid.NewGuid(), Name = "Mateo", Age = 27, email = "mat@gamil.com",password="mati345" }
        };
        
        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAll()
            => Ok(_users);

        
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



    }
}
