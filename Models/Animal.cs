using System;
using System.ComponentModel.DataAnnotations;
public class User
{
    public Guid Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(12)]
    public string Password { get; set; } = string.Empty;

    [Range(0, 110)]
    public int Age { get; set; }
}

// DTOs 
public record CreateUserDto
{
    [Required, StringLength(50)]
    public string Name { get; init; } = string.Empty;

    [Required, EmailAddress]
    public string Gmail { get; set; } = string.Empty;

}

public record UpdateAnimalDto
{
    [Required, StringLength(50)]
    public string Name { get; init; } = string.Empty;

    [Required, EmailAddress]
    public string Gmail { get; set; } = string.Empty;

    [Range(0, 110)]
    public int Age { get; init; }
}