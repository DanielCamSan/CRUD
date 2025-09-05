using Microsoft.AspNetCore.Mvc;
namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> _books = new()
        {
            new Book { Id = Guid.NewGuid(), Title = "Maestry", Publisher = "Coco", year = 2002 },
            new Book { Id = Guid.NewGuid(), Title = "Bank", Publisher = "Santillana", year = 2005 }
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
            var book = _books.FirstOrDefault(b => b.Id == id);
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
            var index = _books.FindIndex(b => b.Id == id);
            if (index == -1) return NotFound();

            book.Id = id; // Mantener el mismo Id
            _books[index] = book;
            return Ok(book);
        }

        // PATCH api/books/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Book> Patch(Guid id, [FromBody] Book partial)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book is null) return NotFound();

            // Solo actualiza si el campo tiene valor
            if (!string.IsNullOrEmpty(partial.Title)) book.Title = partial.Title;
            if (!string.IsNullOrEmpty(partial.Publisher)) book.Publisher = partial.Publisher;
            if (partial.year > 0) book.year = partial.year;

            return Ok(book);
        }

        // DELETE api/books/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _books.RemoveAll(b => b.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}