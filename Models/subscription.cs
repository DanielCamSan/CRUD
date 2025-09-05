using System;
using System.ComponentModel.DataAnnotations;
    public class Subscription
    {
    public Guid id { get; set; }

    [Required, StringLength(100)]
    public string name { get; set; } = string.Empty;
    
    [Required, Range(1,31)]
    public int duracion { get; set; }

    [Required, StringLength(100)]
    public DateTime date { get; set; }  


    }


