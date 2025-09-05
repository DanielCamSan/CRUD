namespace newCRUD.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime InDate { get; set; }
        public int Duration { get; set; }
    }
}