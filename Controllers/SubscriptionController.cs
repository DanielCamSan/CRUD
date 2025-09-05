using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new() { 
           new Subscription {id=Guid.NewGuid(), name = "Rodrigo", date = DateTime.Now, duracion=12},
           new Subscription {id=Guid.NewGuid(), name = "Luis", date = DateTime.Now, duracion=15},
           new Subscription {id=Guid.NewGuid(), name = "Randall", date = DateTime.Now, duracion=30},
        };
        [HttpGet]
        public ActionResult<IEnumerable<Subscription>> GetAll()=> Ok(_subscriptions);
        

    }
}


/*

        // READ: GET api/animals
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
            => Ok(_animals);

        // READ: GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Animal> GetOne(Guid id)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id);
            return animal is null
                ? NotFound(new { error = "Animal not found", status = 404 })
                : Ok(animal);
        }

        // CREATE: POST api/animals
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

        // UPDATE (full): PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Animal> Update(Guid id, [FromBody] UpdateAnimalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _animals.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Animal not found", status = 404 });

            var updated = new Animal
            {
                Id = id,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Age = dto.Age
            };

            _animals[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _animals.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Animal not found", status = 404 })
                : NoContent();
        }
    }
}*/