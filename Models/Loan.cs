namespace CRUD.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
    }
}