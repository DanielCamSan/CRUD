using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private static readonly List<Usuarios> _usus = new()
            {
                new Usuarios { Id = Guid.NewGuid(), Name = "Luna", Apellido = "Smith", Age = 53 },
                new Usuarios { Id = Guid.NewGuid(), Name = "Mishel", Apellido = "Lujan", Age = 22 }
            };

        // GET api/Usuarios
        [HttpGet]
        public ActionResult<IEnumerable<Usuarios>> GetAll()
        {
            return Ok(_usus);
        }

        // GET api/Usuarios/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Usuarios> GetOne(Guid id)
        {
            var user = _usus.FirstOrDefault(a => a.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/animals
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] Usuarios user)
        {
            user.Id = Guid.NewGuid();
            _usus.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] Usuarios user)
        {
            var index = _usus.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            user.Id = id; // conservar el mismo Id
            _usus[index] = user;
            return Ok(user);
        }

        // PATCH api/animals/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Animal> Patch(Guid id, [FromBody] Usuarios user)
        {
            var youser = _usus.FirstOrDefault(a => a.Id == id);
            if (youser is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(user.Name)) youser.Name = user.Name;
            if (!string.IsNullOrEmpty(user.Apellido)) youser.Apellido = user.Apellido;
            if (user.Age > 0) youser.Age = user.Age;

            return Ok(youser);
        }

        // DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _usus.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}