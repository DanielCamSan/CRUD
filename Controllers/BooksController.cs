using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private static readonly List<User> books = new()
            {
                new Book { Id = Guid.NewGuid(), name = "pride and prejudice ", genre = "novel",edition=1 },
                new Book { Id = Guid.NewGuid(), name = "hunger games", genre = "drama",edition=2 }
            };
    }
}
