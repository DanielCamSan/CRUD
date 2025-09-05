using System.ComponentModel.DataAnnotations;

namespace newCRUD.Models
{
    public class User
    {

        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int age { get; set; }

        [Required, StringLength(50)]
        public string email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string password { get; set; } = string.Empty;


    }

  
    public record CreateUserDto
    {
        [Required, StringLength(100)]
        public string name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int age { get; set; }

        [Required, StringLength(50)]
        public string email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string password { get; set; } = string.Empty;
    }

    public record UpdateUserDto
    {
        [Required, StringLength(100)]
        public string name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int age { get; set; }

        [Required, StringLength(50)]
        public string email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string password { get; set; } = string.Empty;
    }
}

