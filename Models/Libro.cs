using System;

namespace newCRUD.Models
{
    public class Libro
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = "";
        public string Autor { get; set; } = "";
        public int AñoPublicacion { get; set; }
        public string Genero { get; set; } = "";
    }
}
