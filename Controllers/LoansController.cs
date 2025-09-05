using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> _loans = new()
            {
                new Loan { Id = Guid.NewGuid(),IdUser = Guid.NewGuid(), IdBook = Guid.NewGuid(), LoanDate= DateTime.Now, DueDate= DateTime.Now.AddDays(3) },
                new Loan { Id = Guid.NewGuid(),IdUser = Guid.NewGuid(), IdBook = Guid.NewGuid(), LoanDate= DateTime.Now.AddDays(2), DueDate= DateTime.Now.AddDays(7)},
                new Loan { Id = Guid.NewGuid(),IdUser = Guid.NewGuid(), IdBook = Guid.NewGuid(), LoanDate= DateTime.Now.AddDays(4), DueDate= DateTime.Now.AddDays(10) }
            };

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
        {
            return Ok(_loans);
        }

        // GET api/animals/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Loan> GetOne(Guid id)
        {
            var loan = _loans.FirstOrDefault(a => a.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        // POST api/animals
        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = Guid.NewGuid();
            _loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }

        // PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Loan> Update(Guid id, [FromBody] Loan loan)
        {
            var index = _loans.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            loan.Id = id; // conservar el mismo Id
            _loans[index] = loan;
            return Ok(loan);
        }

        [HttpPatch("{id:guid}")]
        public ActionResult<Loan> Patch(Guid id, [FromBody] Loan partial)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan is null)
            {
                return NotFound();
            }
            if (partial.IdUser != Guid.Empty)
            {
                loan.IdUser = partial.IdUser;
            }
            if (partial.IdBook != Guid.Empty)
            {
                loan.IdBook = partial.IdBook;
            }
            if (partial.LoanDate != DateTime.MinValue)
            {
                loan.LoanDate = partial.LoanDate;
            }
            if (partial.DueDate != DateTime.MinValue)
            {
                loan.DueDate = partial.DueDate;
            }
            return Ok(loan);
        }

        // DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _loans.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}



    