using System;
using System.ComponentModel.DataAnnotations;
public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Title{ get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Range(1900, 2030)]
    public int Year { get; set; }
}
// DTOs (entrada/salida para la API)
public record CreateMovieDto
{
    [Required, StringLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; init; } = string.Empty;

    [Range(1900, 2030)]
    public int Year { get; init; }
}

public record UpdateMovieDto
{
    [Required, StringLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; init; } = string.Empty;

    [Range(1930, 2030)]
    public int Year { get; init; }
}