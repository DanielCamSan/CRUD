using System;
using System.ComponentModel.DataAnnotations;
public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(0, 400)]
    public int Duration { get; set; }
}

public record CreateMovieDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(0, 400)]
    public int Duration { get; set; }
}
public record UpdateMovieDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(0, 400)]
    public int Duration { get; set; }
}
