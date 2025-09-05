using System;
using System.ComponentModel.DataAnnotations;
public class Subscription
 {
    public Guid Id { get; set; }

    [Required]
    public DateOnly SubscriptionDate { get; set; }

    [Required]
    public int duration { get; set; }

    [Required, StringLength(100)]
    public string name { get; set; } = string.Empty;
}
