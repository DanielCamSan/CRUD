using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> _loans = new()
        {
            new Loan
            {
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                LoanDate = DateTime.UtcNow.AddDays(-10),
                ReturnDate = null 
            },
            new Loan
            {
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                LoanDate = DateTime.UtcNow.AddDays(-30),
                ReturnDate = DateTime.UtcNow.AddDays(-15)
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            return Ok(_loans);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Loan> GetById(Guid id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = Guid.NewGuid();
            loan.LoanDate = DateTime.UtcNow;
            loan.ReturnDate = null;

            _loans.Add(loan);

            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Loan> Update(Guid id, [FromBody] Loan updatedLoan)
        {
            var index = _loans.FindIndex(l => l.Id == id);
            if (index == -1)
            {
                return NotFound();
            }

            updatedLoan.Id = id;
            _loans[index] = updatedLoan;

            return Ok(updatedLoan);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removedCount = _loans.RemoveAll(l => l.Id == id);
            return removedCount == 0 ? NotFound() : NoContent();
        }
    }
}