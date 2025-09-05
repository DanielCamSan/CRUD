using System.ComponentModel.DataAnnotations;

public class User
    {
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    [Range(0, 100)]
    public int Age { get; set; }
}

public record CreateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Password { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Range(0, 120)]
    public int Age { get; init; }
}

public record UpdateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Password { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Range(0, 120)]
    public int Age { get; init; }
}
