using System;
using System.ComponentModel.DataAnnotations;

public class Subscription
{
    public Guid Id { get; set; }
    [Required]
    public DateTime Subscription_date { get; set; }
    [Required,Range(1,365)]
    public int Duration { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
}

public record CreateSubscriptionDto
{
    [Required]
    public DateTime Subscription_date { get; init; } = DateTime.Now;
    [Required, Range(1, 365)]
    public int Duration { get; init; } = 30;
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;
}

public record UpdateSubscriptionDto
{
    [Required]
    public DateTime Subscription_date { get; init; } = DateTime.Now;
    [Required, Range(1, 365)]
    public int Duration { get; init; } = 30;
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;
}