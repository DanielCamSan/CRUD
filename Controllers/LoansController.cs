using Microsoft.AspNetCore.Mvc;
using CRUD.Models;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        // Lista en memoria para simular base de datos
        private static List<Loans> _loans = new List<Loans>();

        // GET: api/loans
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_loans);
        }

        // GET: api/loans/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        // POST: api/loans
        [HttpPost]
        public IActionResult Create([FromBody] Loans loan)
        {
            loan.Id = _loans.Count + 1; // autoincrementar ID
            _loans.Add(loan);

            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        // PUT: api/loans/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Loans updatedLoan)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            loan.User = updatedLoan.User;
            loan.Item = updatedLoan.Item;
            loan.LoanDate = updatedLoan.LoanDate;
            loan.ReturnDate = updatedLoan.ReturnDate;

            return Ok(loan);
        }

        // DELETE: api/loans/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            _loans.Remove(loan);
            return NoContent();
        }
    }
}
