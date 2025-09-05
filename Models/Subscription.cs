using System;
using System.ComponentModel.DataAnnotations;
public class Subscription
{
    public Guid Id { get; set; }

    [Required]
    public DateTime Subscription_Date { get; set; }
    [Required]
    public DateTime Duration { get; set; }
    [StringLength(20), Required]
    public string Name { get; set; } = string.Empty;
    
}

// DTOs (entrada/salida para la API)
public record CreateSubscriptionDto
{
    [Required]
    public DateTime Subscription_Date { get; init; }

    [Required]
    public DateTime Duration { get; init; }

    [StringLength(20), Required]
    public string Name { get; init; } = string.Empty;
}

public record UpdateAnimalDto
{
    [Required]
    public DateTime Subscription_Date { get; init; } 

    [Required]
    public DateTime Duration { get; init; }

    [StringLength(20), Required]
    public string Name { get; init; } = string.Empty;
}