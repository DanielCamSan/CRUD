using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> books = new()
            {
                new Book { Id = Guid.NewGuid(), Name = "La Odisea", Autor = "Homero" },
                new Book { Id = Guid.NewGuid(), Name = "La Divina Comedia", Autor= "Dante" }
            };

        // GET api/animals
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(books);
        }

        // GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Book> GetOne(Guid id)
        {
            var book = books.FirstOrDefault(a => a.Id == id);
            return book is null ? NotFound() : Ok(book);
        }

        // POST api/animals
        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            book.Id = Guid.NewGuid();
            books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { id = book.Id }, book);
        }

        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Book> Update(Guid id, [FromBody] Book book)
        {
            var index = books.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            book.Id = id; // conservar el mismo Id
            books[index] = book;
            return Ok(book);
        }

        // PATCH api/animals/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Book> Patch(Guid id, [FromBody] Book partial)
        {
            var book = books.FirstOrDefault(a => a.Id == id);
            if (book is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) book.Name = partial.Name;
            if (!string.IsNullOrEmpty(partial.Autor)) book.Autor = partial.Autor;
           

            return Ok(book);
        }

        // DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = books.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
