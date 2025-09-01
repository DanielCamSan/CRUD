public record Loans
{
    public Guid id { set; get; }
    public int amount { set; get; }
    public DateTime date { set; get; } = DateTime.Now;
}