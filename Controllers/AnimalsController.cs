using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private static readonly List<Animal> _animals = new()
            {
                new Animal { Id = Guid.NewGuid(), Name = "Luna", Species = "Dog", Age = 3 },
                new Animal { Id = Guid.NewGuid(), Name = "Michi", Species = "Cat", Age = 2 }
            };

        // GET api/animals
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
        {
            return Ok(_animals);
        }

        // GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Animal> GetOne(Guid id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            return animal is null
                ? NotFound(new { error = "Animal not found", status = 404 })
                : Ok(animal);
        }

        // POST api/animals
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] CreateAnimalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Age = dto.Age
            };

            _animals.Add(animal);
            return CreatedAtAction(nameof(GetOne), new { id = animal.Id }, animal);
        }

        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] Animal animal)
        {
            var index = _animals.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            animal.Id = id; // conservar el mismo Id
            _animals[index] = animal;
            return Ok(animal);
        }

        // PATCH api/animals/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Animal> Patch(Guid id, [FromBody] Animal partial)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            if (animal is null) return NotFound();

            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) animal.Name = partial.Name;
            if (!string.IsNullOrEmpty(partial.Species)) animal.Species = partial.Species;
            if (partial.Age > 0) animal.Age = partial.Age;

            return Ok(animal);
        }

        // DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _animals.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
