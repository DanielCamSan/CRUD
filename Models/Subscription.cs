using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

public class Subscription
{
    public Guid Id { get; set; }
    [Required, StringLength(50)]
    public string name { get; set; } = string.Empty;

    [Required]
    public DateOnly subscription_date { get; set; }

    [Required, Range(1, 60)]
    public int duration { get; set; }

}

public record createSubscriptionDto
{
    [Required, StringLength(50)]
    public string name { get; init; } = string.Empty;

    [Required]
    public DateOnly subscription_date { get; init; }

    [Required, Range(1, 60)]
    public int duration { get; init; }
}

public record updateSubscriptionDto
{
    [Required, StringLength(50)]
    public string new_name { get; init; } = string.Empty;

    [Required]
    public DateOnly new_subscription_date { get; init; }

    [Required,Range(1,60)]
    public int new_duration { get; init; }
}
