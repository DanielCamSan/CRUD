public class Loan
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Book { get; set; } = "";
    public DateOnly loanDate { get; set; }
    public DateOnly returnDate { get; set; }
}
