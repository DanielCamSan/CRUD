using Microsoft.AspNetCore.Mvc;

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

    }
}
