using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static readonly List<Loans> _Loans = new()
            {
               new Loans { Id = Guid.NewGuid(), NameBook="Harry Potter", NameUser = "Rodrigo", LoanDays = 4 },
               new Loans { Id = Guid.NewGuid(), NameBook="A Game of Thrones", NameUser = "Adrian", LoanDays = 10 },
               new Loans { Id = Guid.NewGuid(), NameBook="Thinking, Fast and Slow", NameUser = "Andrea", LoanDays = 7 }
            };

        [HttpGet]
        public ActionResult<IEnumerable<Loans>> GetAll()
        {
            return Ok(_Loans);
        }


        [HttpGet("{id:guid}")]
        public ActionResult<Loans> GetOne(Guid id)
        {
            var loans = _Loans.FirstOrDefault(a => a.Id == id);
            return loans is null ? NotFound() : Ok(loans);
        }

        [HttpPost]
        public ActionResult<Loans> Create([FromBody] Loans loans)
        {
            loans.Id = Guid.NewGuid();
            _Loans.Add(loans);
            return CreatedAtAction(nameof(GetOne), new { id = loans.Id }, loans);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Loans> Update(Guid id, [FromBody] Loans loans)
        {
            var index = _Loans.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();

            loans.Id = id; 
            _Loans[index] = loans;
            return Ok(loans);
        }


        [HttpPatch("{id:guid}")]
        public ActionResult<Loans> Patch(Guid id, [FromBody] Loans partial)
        {
            var loans = _Loans.FirstOrDefault(a => a.Id == id);
            if (loans is null) return NotFound();

            if (!string.IsNullOrEmpty(partial.NameBook)) loans.NameBook = partial.NameBook;
            if (!string.IsNullOrEmpty(partial.NameUser)) loans.NameUser = partial.NameUser;
            if (partial.LoanDays > 0) loans.LoanDays = partial.LoanDays;

            return Ok(loans);
        }


        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _Loans.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
