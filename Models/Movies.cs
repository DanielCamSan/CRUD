using System;
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; set; }

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;
}

// DTOs
public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; init; }

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;
}

public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; init; }

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;
}



