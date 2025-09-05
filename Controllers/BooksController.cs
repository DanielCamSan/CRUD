using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private static readonly List<Book> books = new()
            {
                new Book { id = Guid.NewGuid(), name = "pride and prejudice ", genre = "novel",edition=1 }, 
                new Book { id = Guid.NewGuid(), name = "the hunger games", genre = "drama",edition=2 }
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
            book.id = Guid.NewGuid();
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

            books.id = Id;
            books[index] = book;
            return Ok(book);
        }
    // PATCH api/books/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Book> Patch(Guid Id, [FromBody] Book partial)
        {
            var book = books.FirstOrDefault(a => a.id == Id);
            if (book is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.name)) book.name = partial.name;
            if (!string.IsNullOrEmpty(partial.genre)) book.genre = partial.genre;
            book.edition = partial.edition;
            return Ok(book);
        }
   // DELETE api/books/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = books.RemoveAll(a => a.id == Id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
