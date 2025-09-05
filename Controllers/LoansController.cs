using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> _loans = new()
            {
                new Loan { Id = Guid.NewGuid(), Book = "Bible",User = "Carlos", Date = 02112025 },
                new Loan { Id = Guid.NewGuid(), Book = "Comedy",User = "Pedro", Date = 22012025  }
            };

        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            return Ok(_loans);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Loan> GetOne(Guid id)
        {
            var loan = _loans.FirstOrDefault(a => a.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] Loan loan)
        {
            loan.Id = Guid.NewGuid();
            _loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }



    }
    
}
