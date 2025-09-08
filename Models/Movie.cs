using System.ComponentModel.DataAnnotations;
public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; set; }
}

// DTOs (entrada/salida para la API)
public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; init; }
}

public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Genre { get; init; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; init; }
}