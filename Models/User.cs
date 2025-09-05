using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int Age { get; set; }
}
public record CreateUsersDto
{
    [Required, StringLength(100)]
    public string Name { get; init; }= string.Empty;
    [Required, StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Range(0,100)]
    public int Age {  get; set; }
}