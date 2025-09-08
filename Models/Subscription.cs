using System;
using System.ComponentModel.DataAnnotations;

public class Subscription
{
    public Guid Id { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime SubscriptionDate { get; set; }

    [Range(1, 3650)] // entre 1 día y 10 años
    public int Duration { get; set; }
}

// DTOs (entrada/salida para la API)
public record CreateSubscriptionDto
{
    [Required, StringLength(60)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public DateTime SubscriptionDate { get; init; }

    [Range(1, 3650)]
    public int Duration { get; init; }
}

public record UpdateSubscriptionDto
{
    [Required, StringLength(60)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public DateTime SubscriptionDate { get; init; }

    [Range(1, 3650)]
    public int Duration { get; init; }
}
