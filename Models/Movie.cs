using System;
using System.ComponentModel.DataAnnotations;
public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; set; }
}

public record CreateMovieDto
{
    [Required, StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; set; }
}
public record UpdateMovieDto
{
    [Required, StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; set; }
}
