using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> _books = new()
            {
                new Book { Id = Guid.NewGuid(), Title = "Orgullo y prejuicio", Author = "Jane Austen", Year = 1813 },
                new Book { Id = Guid.NewGuid(), Title = "Cien años de soledad", Author = "Gabriel Garcia", Year = 1967 }
            };

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(_books);
        }

        // GET api/books/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Book> GetOne(Guid id)
        {
            var book = _books.FirstOrDefault(a => a.Id == id);
            return book is null ? NotFound() : Ok(book);
        }

        // POST api/books
        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            book.Id = Guid.NewGuid();
            _books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { id = book.Id }, book);
        }

        // PUT api/books/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Book> Update(Guid id, [FromBody] Book book)
        {
            var index = _books.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            book.Id = id; // conservar el mismo Id
            _books[index] = book;
            return Ok(book);
        }

        // DELETE api/books/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _books.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
