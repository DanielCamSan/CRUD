using System;
using System.ComponentModel.DataAnnotations;

namespace newCRUD.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime SubscriptionDate { get; set; }

        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 months.")]
        public int DurationMonths { get; set; }
    }

    // DTOs para entrada/salida
    public record CreateSubscriptionDto
    {
        [Required, StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [Required]
        public DateTime SubscriptionDate { get; init; }

        [Range(1, 120)]
        public int DurationMonths { get; init; }
    }

    public record UpdateSubscriptionDto
    {
        [Required, StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [Required]
        public DateTime SubscriptionDate { get; init; }

        [Range(1, 120)]
        public int DurationMonths { get; init; }
    }
}
