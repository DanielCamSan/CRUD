using Microsoft.AspNetCore.Mvc;
public class Loan
{
    public Guid Id { get; set; }
    public string Borrower { get; set; } = "";   // Nombre del prestatario
    public string BookTitle { get; set; } = "";  // Título del libro
    public DateTime LoanDate { get; set; }       // Fecha de préstamo
    public DateTime? ReturnDate { get; set; }    // Fecha de devolucion (puede ser null si no devolvió)
}