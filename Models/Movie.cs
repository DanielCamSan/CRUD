using System;
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(1950, 2025)]
    public int Year { get; set; }
}

public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Range(1950, 2025)]
    public int Year { get; init; }
}


public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title  { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Range(1950, 2025)]
    public int Year { get; init; }
}
