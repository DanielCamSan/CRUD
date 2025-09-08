using System;
using System.ComponentModel.DataAnnotations;
public class User
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Passsword { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; set; }
}

// DTOs (entrada/salida para la API)
public record CreateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Password { get; init; } = string.Empty;

}

public record UpdateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }
}