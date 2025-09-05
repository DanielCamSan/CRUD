public class Book
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public string Publisher { get; set; } = "";
    public int YearPublication {get; set;}
}