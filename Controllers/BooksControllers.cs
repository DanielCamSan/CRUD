using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Books> _books = new()
        {
            new Books { Id = Guid.NewGuid(), Title = "Cien años de soledad", Author = "Gabriel García Márquez", Year = 1967 },
            new Books { Id = Guid.NewGuid(), Title = "El Quijote", Author = "Miguel de Cervantes", Year = 1605 }
        };

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<Books>> GetAll()
        {
            return Ok(_books);
        }

        // GET api/books/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Books> GetOne(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return book is null ? NotFound() : Ok(book);
        }

        // POST api/books
        [HttpPost]
        public ActionResult<Books> Create([FromBody] Books book)
        {
            book.Id = Guid.NewGuid();
            _books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { id = book.Id }, book);
        }

        // PUT api/books/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Books>Update(Guid id, [FromBody] Books book)
        {
            var index = _books.FindIndex(b => b.Id == id);
            if (index == -1) return NotFound();

            book.Id = id; // mantener el mismo Id
            _books[index] = book;
            return Ok(book);
        }

        // PATCH api/books/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Books> Patch(Guid id, [FromBody] Books partial)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.Title)) book.Title = partial.Title;
            if (!string.IsNullOrEmpty(partial.Author)) book.Author = partial.Author;
            if (partial.Year > 0) book.Year = partial.Year;

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
