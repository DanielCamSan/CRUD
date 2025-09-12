using System;
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; }

    [Required, StringLength(50)]
    public string Genre { get; set; }

    [Required, Range(2000, 2026)]
    public int Year { get; set; }
}

public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Required, Range(2000, 2026)]
    public int Year { get; init; }
}

public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Required, Range(2000, 2026)]
    public int Year { get; init; } 
}