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
    // GET api/books/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Book> GetOne(Guid Id)
        {
            var Book = books.FirstOrDefault(a => a.id == Id);
            return Book is null ? NotFound() : Ok(Book);
        }
     // POST api/books
        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            book.Id = Guid.NewGuid();
            books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { id = book.Id }, book); 
        }
    // PUT api/books/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Book> Update(Guid Id, [FromBody] Book book)
        {
            var index = books.FindIndex(a => a.id == Id);
            if (index == -1) 
            return NotFound();

            books.Id = id;
            books[index] = book;
            return Ok(book);
        }
    }
