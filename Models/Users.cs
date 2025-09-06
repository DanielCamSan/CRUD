using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string email { get; set; } = string.Empty;

    [Range(0, 122)]
    public int Age { get; set; }

    [Required, StringLength(128)]
    public string  password { get; set; } = "";


}
public record CreateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, EmailAddress]
    public string email { get; init; } = string.Empty;

    [Required, StringLength(128)]
    public string password { get; init; } = "";
}
public record UpdateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, EmailAddress]
    public string email { get; init; } = string.Empty;
    
    [Required, StringLength(128)]
    public string password { get; init; } = "";
}
