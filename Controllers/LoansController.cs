using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loan> loans = new()
        {
            new Loan( Id = Guid.NewGuid(), Name = "Dabner", Book = "Holes", loanDate = DateOnly.FromDateTime(DateTime.Now), returnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            new Loan( Id = Guid.NewGuid(), Name = "Andy", Book = "The Giver", loanDate = DateOnly.FromDateTime(DateTime.Now), returnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7))
        };

        //GET api/loans
        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            return Ok(loans);
        }

        //GET api/loans/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Loan> GetOne(Guid id)
        {
            var loan = loans.FirstOrDefault(l => l.Id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        //POST api/loan
        [HttpPost]
        public ActionResult<Loan> Create([FromBody]Loan loan)
        {
            loan.Id = Guid.NewGuid();
            loans.Add(loan);
            return CreatedAtAction(nameof(GetOne), new { id = loan.Id }, loan);
        }

        //PUT api/loan/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Loan> Update(Guid id, [FromBody] Loan loan) 
        {
            var index = loans.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            loan.Id = id;
            loans[index] = loan;
            return Ok(loan);
        }

        //PATCH api/loans/{id}
        [HttpPatch("{id:guid}")]
        public ActionResult<Loan> PATCH(Guid id, [FromBody] Loan partial)
        {
            var loan = loans.FirstOrDefault(a => a.Id == id);
            if (loan is null) return NotFound();

            if(!string.IsNullOrEmpty(partial.Name)) loan.Name = partial.Name;
            if(!string.IsNullOrEmpty(partial.Book)) loan.Book = partial.Book;

            return Ok(loan);
        }


    }
}
