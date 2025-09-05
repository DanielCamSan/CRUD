public class Loan
{
    public Guid Id { get; set; }
    public Guid IdUser { get; set; }
    public Guid IdBook { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate {  get; set; } 

}
