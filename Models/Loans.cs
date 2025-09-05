using System;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Loans
    {
        public int Id { get; set; }   // ID numérico autoincremental

        [Required, StringLength(100)]
        public string User { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Item { get; set; } = string.Empty;

        [Required]
        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
