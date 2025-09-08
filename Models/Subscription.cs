using System;
using System.ComponentModel.DataAnnotations;
public class Subscription
{
    public Guid Id { get; set; }

    [Required]
    public DateTime SubscriptionDate { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [StringLength(20), Required]
    public string Name { get; set; } = string.Empty;
    
}

// DTOs (entrada/salida para la API)
public record CreateSubscriptionDto
{
    [Required]
    public DateTime SubscriptionDate { get; init; }

    [Required]
    public TimeSpan Duration { get; init; }

    [StringLength(20), Required]
    public string Name { get; init; } = string.Empty;
}

public record UpdateSubscriptionDto
{
    [Required]
    public DateTime SubscriptionDate { get; init; } 

    [Required]
    public TimeSpan Duration { get; init; }

    [StringLength(20), Required]
    public string Name { get; init; } = string.Empty;
}