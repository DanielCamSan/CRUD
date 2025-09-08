using System.ComponentModel.DataAnnotations;

namespace newCRUD.Models // <-- Asegúrate de que el namespace sea correcto.
{
    public record CreateSubscriptionDto
    (
        [Required]
        [StringLength(100)]
        string Name,

        [Required]
        DateTime InDate,

        [Required]
        [Range(1, 365)]
        int Duration
    );

    public record UpdateSubscriptionDto
    (
        [Required]
        [StringLength(100)]
        string Name,

        [Required]
        DateTime InDate,

        [Required]
        [Range(1, 365)]
        int Duration
    );
}
