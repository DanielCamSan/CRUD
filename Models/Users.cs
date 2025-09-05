using System;
using System.ComponentModel.DataAnnotations;
public class Users
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; set; }

    [Required, StringLength(100)]
    public string email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string password { get; set; } = string.Empty;
}

// DTOs (entrada/salida para la API)
public record CreateUsersDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, StringLength(100)]
    public string email { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string password { get; init; } = string.Empty;
}
public record UpdateUsersDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, StringLength(100)]
    public string email { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string password { get; init; } = string.Empty;
}
