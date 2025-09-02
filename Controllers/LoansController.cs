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
        



     }



}