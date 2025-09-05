 Book  Publisher = "Veron", YearPublication = 1972 },
                new Book { Id = Guid.NewGuid(), Name = "The Count of Monte Cristo", Author = "Alexander Dumas", Publisher = "SAS", YearPublication = 1846 }
            };
public class Book
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public string Publisher { get; set; } = "";
    public int YearPublication { get; set; }
}