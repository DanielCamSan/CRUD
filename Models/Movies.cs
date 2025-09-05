using System;
using System.ComponentModel.DataAnnotations;

namespace newCRUD.Models
{
    public class Movie
    {
        public Guid Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(60)]
        public string Genre { get; set; } = string.Empty;

        [Range(1888, 2100)]
        public int Year { get; set; }
    }
}