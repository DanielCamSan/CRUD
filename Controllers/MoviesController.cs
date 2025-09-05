
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController: ControllerBase
    {
        private static readonly List<Movies> _movies = new()
        {
            new Movies { Title = "Conjuro4", Gender = "Horror", Year = 2025 },
            new Movies { Title = "Bestia", Gender = "Romantic", Year = 2020 }
        };




    }
}

       