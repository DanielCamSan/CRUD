using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : Controller
    {
        private static readonly List<Loans> loans = new()
        {
            new Loans{id = Guid.NewGuid(), amount=100,date=DateTime.Now},
            new Loans{id = Guid.NewGuid(), amount=200,date=DateTime.Now},
            new Loans{id = Guid.NewGuid(), amount=300,date=DateTime.Now},
        };

        [HttpGet]
        public ActionResult<IEnumerable<Loans>> GetAll()
        {
            return Ok(loans);
        }

        [HttpGet("{id::guid}")]
        public ActionResult<Loans> GetById(Guid id)
        {
            var loan = loans.FirstOrDefault(p => p.id == id);
            return loan is null ? NotFound() : Ok(loan);
        }

        [HttpPost]
        public ActionResult<Loans> Create([FromBody] Loans loan)
        {
            loan.id = Guid.NewGuid();
            loans.Add(loan);
            return CreatedAtAction(nameof(GetById), new { id = loan.id}, loan); ;
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Loans> Update(Guid id, [FromBody] Loans loan)
        {
            var index = loans.FindIndex(l => l.id == id);
            if (index == -1) return NotFound();

            loan.id = id;
            loans[index] = loan;
            return Ok(loan);
        }

        [HttpPatch("{id:guid}")]
        public ActionResult<Loans> Patch(Guid id, [FromBody] Loans partial)
        {
            var loan = loans.FirstOrDefault(l => l.id == id);
            if (loan is null) return NotFound();

            if (partial.amount > 0) loan.amount = partial.amount;
            return Ok(partial);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = loans.RemoveAll(l => l.id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
