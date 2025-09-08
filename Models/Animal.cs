
using System;
using System.ComponentModel.DataAnnotations;
public class Animal
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Species { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; set; }
}

// DTOs (entrada/salida para la API)
public record CreateAnimalDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Species { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }
}

public record UpdateAnimalDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Species { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }
}