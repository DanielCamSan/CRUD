
    public class Animal
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Species { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Age { get; set; }
    }

    // DTOs para entrada
    public record CreateAnimalDto(
        [property: Required, StringLength(100)] string Name,
        [property: Required, StringLength(50)] string Species,
        [property: Range(0, 100)] int Age
    );

    public record UpdateAnimalDto(
        [property: Required, StringLength(100)] string Name,
        [property: Required, StringLength(50)] string Species,
        [property: Range(0, 100)] int Age
    );
