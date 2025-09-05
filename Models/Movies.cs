using System;
using System.ComponentModel.DataAnnotations;
public class Movies
{

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Gender { get; set; } = string.Empty;

    [Range(0, 2025)]
    public int Year { get; set; }

}


// DTOs (entrada/salida para la API)
public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Gender { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Year { get; init; }
}

public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Gender { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Year { get; init; }
}