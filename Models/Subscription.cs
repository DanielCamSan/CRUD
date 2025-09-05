using System.ComponentModel.DataAnnotations;

public class Subscription
{
    public Guid Id { get; set; }

    [Required]
    public DateTime SubscriptionDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
    public int Duration { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
}


public record CreateSubscriptionDto
{
    [Required]
    public DateTime SubscriptionDate { get; init; }

    [Required]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
    public int Duration { get; init; }

    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;
}

public record UpdateSubscriptionDto
{
    [Required]
    public DateTime SubscriptionDate { get; init; }

    [Required]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
    public int Duration { get; init; }

    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;
}