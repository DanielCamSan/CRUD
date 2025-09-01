using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> _loans = new()
        {
            new Loan { Id = Guid.NewGuid(), Borrower = "Juan Perez", BookTitle = "Cien años de soledad", LoanDate = DateTime.Now.AddDays(-5), ReturnDate = null },
            new Loan { Id = Guid.NewGuid(), Borrower = "Ana Lopez", BookTitle = "El Quijote", LoanDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now.AddDays(-2) }
        };

        // GET api/loans
        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            return Ok(_loans);
        }

        // GET api/loans/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Loan> GetOne(Guid id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        // POST api/loans
        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = Guid.NewGuid();
            loan.LoanDate = DateTime.Now; // asigna la fecha actual al crear
            _loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }

        // PUT api/loans/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Loan> Update(Guid id, [FromBody] Loan loan)
        {
            var index = _loans.FindIndex(l => l.Id == id);
            if (index == -1) return NotFound();

            loan.Id = id; // conservar el mismo Id
            _loans[index] = loan;
            return Ok(loan);
        }

        // PATCH api/loans/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Loan> Patch(Guid id, [FromBody] Loan partial)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.Borrower)) loan.Borrower = partial.Borrower;
            if (!string.IsNullOrEmpty(partial.BookTitle)) loan.BookTitle = partial.BookTitle;
            if (partial.LoanDate != default) loan.LoanDate = partial.LoanDate;
            if (partial.ReturnDate != null) loan.ReturnDate = partial.ReturnDate;

            return Ok(loan);
        }

        // DELETE api/loans/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _loans.RemoveAll(l => l.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}