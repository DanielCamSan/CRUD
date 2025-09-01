namespace newCRUD.Models
{
    public class Loan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; }
        // ReturnDate is nullable because a loan might not be returned yet.
        public DateTime? ReturnDate { get; set; }
    }
}