using System;
using System.ComponentModel.DataAnnotations;
public class Subscription
 {
    public Guid Id { get; set; }

    [Required]
    public DateOnly SubscriptionDate { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    //DTOs
    public record CreateSubscriptionDto
    {
        [Required, StringLength(100)]
        public string Name { get; init; } = string.Empty;
        [Required, Range(30, 365)]
        public int Duration { get; init; }
    }

    public record UpdateSubscriptionDto
    {
        [Required, StringLength(100)]
        public string Name { get; init; } = string.Empty;
        [Required]
        public int Duration { get; init; }
    }
}
