using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> _loans = new()
        {
            new Loan { Id=Guid.NewGuid(), IdUser = Guid.NewGuid(),  IdBook = Guid.NewGuid(), LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(3) },
            new Loan { Id=Guid.NewGuid(), IdUser = Guid.NewGuid(),  IdBook = Guid.NewGuid(), LoanDate = DateTime.Now.AddDays(2), DueDate = DateTime.Now.AddDays(7) },
            new Loan { Id=Guid.NewGuid(), IdUser = Guid.NewGuid(),  IdBook = Guid.NewGuid(), LoanDate = DateTime.Now.AddDays(4), DueDate = DateTime.Now.AddDays(10) },
        };


        [HttpGet("{id}")]
        public ActionResult<Loan> GetOne(Guid id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null)
            {
                return NotFound();
            }
            return Ok(loan);
        }


        // POST api/loans
        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = Guid.NewGuid();
            _loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }

        // PATCH api/loans/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Loan> Patch(Guid id, [FromBody] Loan partial)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            // Actualiza solo si los valores recibidos son válidos
            if (partial.IdUser != Guid.Empty) loan.IdUser = partial.IdUser;
            if (partial.IdBook != Guid.Empty) loan.IdBook = partial.IdBook;
            if (partial.LoanDate != default) loan.LoanDate = partial.LoanDate;
            if (partial.DueDate != default) loan.DueDate = partial.DueDate;

            return Ok(loan);
        }

        //DELETE api/loans/{id}
        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var index = _loans.FindIndex(l => l.Id == id);
            if (index == -1) return NotFound();
            _loans.RemoveAt(index);
            return NoContent();
        }


    }
}