using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using tecWebGrupo7.CRUD.Models;

namespace tecWebGrupo7.CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private static List<Loan> _loans = new();
        private static int _nextId = 1;

        // GET /api/loans
        [HttpGet]
        public IEnumerable<Loan> GetAll() => _loans;

        // GET /api/loans/{id}
        [HttpGet("{id}")]
        public ActionResult<Loan> GetById(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            return loan is null ? NotFound() : loan;
        }

        // POST /api/loans
        [HttpPost]
        public Loan Create(Loan loan)
        {
            loan.Id = _nextId++;
            _loans.Add(loan);
            return loan;
        }

        // PUT /api/loans/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Loan input)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan is null) return NotFound();

            loan.UserId = input.UserId;
            loan.BookId = input.BookId;
            loan.DueDate = input.DueDate;
            loan.ReturnDate = input.ReturnDate;

            return NoContent();
        }

        // DELETE /api/loans/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan is null) return NotFound();

            _loans.Remove(loan);
            return NoContent();
        }
    }
}
