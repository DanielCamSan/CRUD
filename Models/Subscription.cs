using System.ComponentModel.DataAnnotations;
namespace newCRUD.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime InDate { get; set; }

        [Range(1, 365)]
        public int Duration { get; set; }
    }

    public record CreateSubscriptionDto
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; init; } = string.Empty;

        [Required]
        public DateTime InDate { get; init; }

        [Required, Range(1, 365)]
        public int Duration { get; init; }
    }

    public record UpdateSubscriptionDto
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; init; } = string.Empty;

        [Required]
        public DateTime InDate { get; init; }

        [Required, Range(1, 365)]
        public int Duration { get; init; }
    }
}