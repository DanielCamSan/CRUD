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
                new Book { Id = Guid.NewGuid(), name = "the hunger games", genre = "drama",edition=2 }
            };
    }
     // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(books);
        }
    }
