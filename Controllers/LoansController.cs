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



     }



}