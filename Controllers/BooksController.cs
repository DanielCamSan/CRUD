using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController:ControllerBase
    {
        private static readonly List<Book> _books = new()
            {
                new Book {title="To_Kill_a_Mockingbird",description="A classic novel set in the American South, exploring themes of justice, race, and morality through the eyes of a young girl.",authors = "Harper Lee",year_of_publication = 1960},
                new Book {title="To_Kill_a_Mockingbird", description="A classic novel set in the American South, exploring themes of justice, race, and morality through the eyes of a young girl.", authors="Harper Lee", year_of_publication=1960},
                new Book {title="1984", description="A dystopian novel about a totalitarian regime that uses surveillance, censorship, and control to oppress its people.", authors="George Orwell", year_of_publication=1949},
                new Book {title="The_Great_Gatsby", description="A story about wealth, love, and the American Dream during the Jazz Age, centered around the mysterious Jay Gatsby.", authors="F. Scott Fitzgerald", year_of_publication=1925},
            };
        [HttpGet]
        public ActionResult<IEnumerable<Book>> getAll()
        {
            return Ok(_books);
        }
        [HttpGet("{title}")]
        public ActionResult<Book> GetOne(string Title)
        {
            var book = _books.FirstOrDefault(b => b.title == Title);
            return book is null ? NotFound() : Ok(book);
        }
        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            _books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { title = book.title }, book);
        }
        [HttpPut("{title}")]
        public ActionResult<Book> Update(string title, [FromBody] Book book)
        {
            var index = _books.FindIndex(a => a.title == title);
            if (index == -1) return NotFound();
            book.title = title;
            _books[index] = book;
            return Ok(book);
        }
        [HttpPatch("{title}")]
        public ActionResult<Book> Patch(string title, [FromBody] Book partial)
        {
            var book = _books.FirstOrDefault(b => b.title == title);
            if (book is null) return NotFound();
            if (!string.IsNullOrEmpty(partial.title)) book.title = partial.title;
            if (!string.IsNullOrEmpty(partial.authors)) book.authors = partial.authors;
            if (!string.IsNullOrEmpty(partial.description)) book.description = partial.description;
            if (partial.year_of_publication > 0) book.year_of_publication = partial.year_of_publication;
            return Ok(book);
        }
        [HttpDelete("{title}")]
        public ActionResult Delete(string title)
        {
            var removed = _books.RemoveAll(b => b.title == title);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
