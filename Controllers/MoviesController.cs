
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController: ControllerBase
    {
        private static readonly List<Movies> _movies = new()
        {
            new Movies { Id = Guid.NewGuid(), Title = "Conjuro4", Gender = "Horror", Year = 2025 },
            new Movies { Id = Guid.NewGuid(), Title = "Bestia", Gender = "Romantic", Year = 2020 }
        };

        // READ: GET api/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movies>> GetAll()
            => Ok(_movies);

       
    }
}


   
       