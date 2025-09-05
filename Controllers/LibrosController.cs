using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private static readonly List<Libro> _libros = new()
        {
            new Libro { Id = Guid.NewGuid(), Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", AñoPublicacion = 1967, Genero = "Realismo mágico" },
            new Libro { Id = Guid.NewGuid(), Titulo = "1984", Autor = "George Orwell", AñoPublicacion = 1949, Genero = "Ciencia ficción" }
        };

        // GET api/libros
        [HttpGet]
        public ActionResult<IEnumerable<Libro>> GetAll()
        {
            return Ok(_libros);
        }

        // GET api/libros/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Libro> GetOne(Guid id)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            return libro is null ? NotFound() : Ok(libro);
        }

        // POST api/libros
        [HttpPost]
        public ActionResult<Libro> Create([FromBody] Libro libro)
        {
            libro.Id = Guid.NewGuid();
            _libros.Add(libro);
            return CreatedAtAction(nameof(GetOne), new { id = libro.Id }, libro);
        }

        // PUT api/libros/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Libro> Update(Guid id, [FromBody] Libro libro)
        {
            var index = _libros.FindIndex(l => l.Id == id);
            if (index == -1) return NotFound();

            libro.Id = id; // conservar el mismo Id
            _libros[index] = libro;
            return Ok(libro);
        }

        // PATCH api/libros/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Libro> Patch(Guid id, [FromBody] Libro partial)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            if (libro is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Titulo)) libro.Titulo = partial.Titulo;
            if (!string.IsNullOrEmpty(partial.Autor)) libro.Autor = partial.Autor;
            if (partial.AñoPublicacion > 0) libro.AñoPublicacion = partial.AñoPublicacion;
            if (!string.IsNullOrEmpty(partial.Genero)) libro.Genero = partial.Genero;

            return Ok(libro);
        }

        // DELETE api/libros/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _libros.RemoveAll(l => l.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}