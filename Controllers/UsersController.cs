using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<Users> users = new()
        {
            new Users { Id = Guid.NewGuid(), Name = "Juan", Edad = 25 },
            new Users { Id = Guid.NewGuid(), Name = "Maria", Edad = 30 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAll()
        {
            return Ok(users);
        }
        [HttpGet("{id:guid}")]

        public ActionResult<Users> GetOne(Guid id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

       
    }
}


    