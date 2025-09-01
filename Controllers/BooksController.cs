using Microsoft.AspNetCore.Mvc;
namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController: ControllerBase
    {

        private static readonly List<Books> _books = new()
        {
            new Books { Id = Guid.NewGuid(), autor = "Gabriel Garcia Marquez", year = 1967, title = "Cien años de soledad" },
            new Books { Id = Guid.NewGuid(), autor = "Isabel Allende", year = 1982, title = "La casa de los espíritus" },
            new Books { Id = Guid.NewGuid(), autor = "Mario Vargas Llosa", year = 1967, title = "La ciudad y los perros" }  
        };

        [HttpGet]
        //it gets all the books 
        public ActionResult <IEnumerable<Books>> GetAll()
        {
            return Ok(_books);
        }

        [HttpGet("{id:guid}")]
        //it gets an specific book by id
        public ActionResult<Books> GetOne(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public ActionResult<Books> Create([FromBody] Books book)
        {
            book.Id = Guid.NewGuid();
            _books.Add(book);
            return CreatedAtAction(nameof(GetOne), new { id = book.Id }, book);
        }


        [HttpPut("{id:guid}")]
        public ActionResult<Books> Update(Guid id, [FromBody] Books book)
        {
            var index = _books.FindIndex(b => b.Id == id);
            if (index == -1) return NotFound();
            book.Id = id; // conservar el mismo Id
            _books[index] = book;
            return Ok(book);
        }


        [HttpPatch("{id:guid}")]
        public ActionResult<Books> Patch(Guid id, [FromBody] Books partial)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.autor)) book.autor = partial.autor;
            if (partial.year != 0) book.year = partial.year;
            if (!string.IsNullOrEmpty(partial.title)) book.title = partial.title;

            return Ok(book);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var removed= _books.RemoveAll(a=> a.Id == id);
            return removed == 0 ? NotFound(): NoContent();
        }
    }
}



