
using System;
using System.ComponentModel.DataAnnotations;
public class User
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public EmailAddressAttribute Email { get; set; }

    [Range(0, 100)]
    public int Age { get; set; }
}