using CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> loans = new();
        private static int nextId = 1;

        public LoansController()
        {
            if (!loans.Any())
            {
                loans.Add(new Loan
                {
                    Id = 1,
                    BookTitle = "Clean Code",
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    LoanDate = DateTime.UtcNow.AddDays(-5),
                    ReturnDate = null
                });
                loans.Add(new Loan
                {
                    Id = 2,
                    BookTitle = "The Pragmatic Programmer",
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    LoanDate = DateTime.UtcNow.AddDays(-2),
                    ReturnDate = DateTime.UtcNow
                });


            }

        }

        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            return Ok(loans);
        }
        [HttpGet("{id:int}")]
        public ActionResult<Loan> GetOne(int id)
        {
            var loan = loans.FirstOrDefault(l => l.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }
        [HttpPost]
        public ActionResult<Loan> Create([FromBody] Loan loan)
        {
            loan.Id = nextId++;
            loan.LoanDate = DateTime.UtcNow;
            loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }
        [HttpPut("{id:int}")]
        public ActionResult<Loan> Update(int id, [FromBody] Loan loan)
        {
            var index = loans.FindIndex(l => l.Id == id);
            if (index == -1) return NotFound();
            loan.Id = id;
            loans[index] = loan;
            return Ok(loan);
        }
        [HttpPatch("{id:int}")]
        public ActionResult<Loan> Patch(int id, [FromBody] Loan partial)
        {
            var loan = loans.FirstOrDefault(l => l.Id == id);
            if (loan is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.BookTitle)) loan.BookTitle = partial.BookTitle;
            if (partial.UserId != Guid.Empty) loan.UserId = partial.UserId;
            if (partial.ReturnDate.HasValue) loan.ReturnDate = partial.ReturnDate;

            return Ok(loan);
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var removed = loans.RemoveAll(l => l.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }




     }



}