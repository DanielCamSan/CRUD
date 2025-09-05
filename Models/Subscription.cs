
using System;
using System.ComponentModel.DataAnnotations;
public class Subscription
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 12)]
    public int duration { get; set; }

    [Required, StringLength(20)]
    public string subcription_date { get; set; } = string.Empty;

   
}

public record CreateSubscriptionDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 12)]
    public int duration { get; init; }

    [Required, StringLength(20)]
    public string subcription_date { get; init; } = string.Empty;
}

public record UpdateSubscriptionDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 12)]
    public int duration { get; init; }

    [Required, StringLength(20)]
    public string subcription_date { get; init; } = string.Empty;
}