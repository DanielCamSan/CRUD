public record Loans
{
    public Guid id { set; get; }
    public string bookName { set; get; } = string.Empty;
    public string customerName { set; get; } = string.Empty;
    public DateTime date { set; get; } = DateTime.Now;
}