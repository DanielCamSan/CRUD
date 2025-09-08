Movie API for a cinema
A simple ASP.NET Core Web API project that demonstrates basic CRUD operations on a list of movies.
It includes support for paging, sorting, and filtering through query parameters.

Features
Create, Read, Update, and Delete users (Movie entity).
In-memory storage (List<Movie>) for demo purposes.
Query parameters for:
Paging (page, limit)
Sorting (sort, order)
Search (id, Name, Title, Genre)
Input validation with Data Annotations.
Returns metadata (page, limit, total) in list endpoints.
Endpoints
Base URL:

 http://localhost:5000/api/movies

Method	Endpoint	Description
GET	/api/movies	Get all movies (with paging/filter)
GET	/api/movies/{id}	Get a single movie by ID
POST	/api/movies	Create a new movie
PUT	/api/movies/{id}	Update an existing movie
DELETE	/api/movies/{id}	Delete a movie
Models
Movie
public class Movie
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; set; }
}
public record CreateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; init; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; init; }
}
public record UpdateMovieDto
{
    [Required, StringLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Genre { get; init; } = string.Empty;

    [Range(0, 3000)]
    public int Year { get; init; }
}
Running the project
Clone the repository
git clone https://github.com/your-username/newCRUD.git
cd newCRUD
Restore dependencies
dotnet restore
Run the API
dotnet run
Open in Postman
 http://localhost:5000/api/movies
