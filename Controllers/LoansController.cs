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
    }
}
