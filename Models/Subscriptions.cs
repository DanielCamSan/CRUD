using System;
using System.ComponentModel.DataAnnotations;

public class Subscription
{
    public Guid Id { get; set; }

    [Required, StringLength(80, ErrorMessage = "El nombre no puede exceder 80 caracteres")]
    public string Name { get; set; } = string.Empty;

    // meses (1..60 como regla de ejemplo)
    [Required, Range(1, 60, ErrorMessage = "La duración debe estar entre 1 y 60 meses")]
    public int Duration { get; set; }

    // Solo fecha (yyyy-MM-dd). Si quieres forzar “no futuro”, valida en Controller.
    [Required, DataType(DataType.Date)]
    public DateTime SubscriptionDate { get; set; }
}