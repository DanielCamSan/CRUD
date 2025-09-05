using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace LoansControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loans> list_loans = new()
        {
            new Loans{Id = Guid.NewGuid(), Name = "Gabriel",borrowedBook = "El principito",  LoanDate= new DateOnly(2000, 8, 10)},
            new Loans{Id = Guid.NewGuid(), Name = "Adrian",borrowedBook = "La piramide roja", LoanDate= new DateOnly(2000, 8, 10)}
        };
        [HttpGet]
        // api/loans
        public ActionResult<List<Loans>> getAll()
        {
            return list_loans;
        }

        // api/loans/{id:guidid} 
        [HttpGet("{id:guid}")]
        public ActionResult<Loans> getById(Guid id)
        {
            var author = list_loans.FirstOrDefault(a => a.Id == id);
            return author is null ? NotFound() : Ok(author);
        }

        [HttpPost]
        // api/loans
        public ActionResult<Loans> CreateAuthor([FromBody] Loans loans)
        {
            loans.Id = Guid.NewGuid();
            loans.LoanDate = new DateOnly(loans.LoanDate.Year, loans.LoanDate.Month, loans.LoanDate.Day); 
            list_loans.Add(loans);
            return loans;
        }

        [HttpPatch("{id:guid}")]
        // api/loans
        public ActionResult<string> PatchAuthor(Guid id, [FromBody] Loans partial)
        {
            var author = list_loans.FirstOrDefault(a => a.Id == id);
            if (author is null) return NotFound();
            // solo cambia si trae valor
            if (!string.IsNullOrEmpty(partial.Name)) author.Name = partial.Name;
            if (partial.LoanDate != DateOnly.MinValue)
            {
                author.LoanDate = partial.LoanDate;
            }
            return Ok(author);
        }

        // DELETE api/loans/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = list_loans.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}