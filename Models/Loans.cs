public class Loans
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    // modulo de libros prestados 
    public string borrowedBook { get; set; } = ""; 
    public DateOnly LoanDate { get; set; }
}