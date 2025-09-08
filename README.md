# user API for a cinema

A simple ASP.NET Core Web API project that demonstrates basic CRUD operations on a list of users.  
It includes support for paging, sorting, and filtering through query parameters.

## Features

- Create, Read, Update, and Delete users (`User` entity).
- In-memory storage (`List<User>`) for demo purposes.
- Query parameters for:
    - **Paging** (`page`, `limit`)
    - **Sorting** (`sort`, `order`)
    - **Search** (`q`, `Name`, `email`, `Age`)
- Input validation with **Data Annotations**.
- Returns metadata (`page`, `limit`, `total`) in list endpoints.

## Endpoints

Base URL: 

http://localhost:5107/api/users

| Method | Endpoint              | Description                        |
|--------|-----------------------|------------------------------------|
| GET    | `/api/users`          | Get all users (with paging/filter) |
| GET    | `/api/users/{id}`     | Get a single user by ID            |
| POST   | `/api/users`          | Create a new user                  |
| PUT    | `/api/users/{id}`     | Update an existing user            |
| DELETE | `/api/users/{id}`     | Delete a user                      |

## Models

### `User`
```csharp
public class User
{
    public Guid Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string email { get; set; }
    [Range(0, 122)]
    public int Age { get; set; }
    [Required, StringLength(128)]
    public string password { get; set; }
}
public record CreateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, EmailAddress]
    public string email { get; init; } = string.Empty;

    [Required, StringLength(128)]
    public string password { get; init; } = "";
}
public record UpdateUserDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100)]
    public int Age { get; init; }

    [Required, EmailAddress]
    public string email { get; init; } = string.Empty;
    
    [Required, StringLength(128)]
    public string password { get; init; } = "";
}
```
## Notes

- Currently uses in-memory storage (`List<User>`). All data resets on restart.  
- Passwords are stored without strong hashing. Use SHA256, BCrypt, or PBKDF2 for production.  
- For production, replace `_users` with a database (EF Core, SQL Server, PostgreSQL, etc.).

# Movie APIfor a cinema
A simple ASP.NET Core Web API project that demonstrates basic CRUD operations on a list of movies. It includes support for paging, sorting, and filtering through query parameters.

## Features 
Create, Read, Update, and Delete users (Movie entity). In-memory storage (List) for demo purposes. Query parameters for: Paging (page, limit) Sorting (sort, order) Search (id, Name, Title, Genre) Input validation with Data Annotations. Returns metadata (page, limit, total) in list endpoints. Endpoints Base URL:

http://localhost:5000/api/movies

Method Endpoint Description GET /api/movies Get all movies (with paging/filter) GET /api/movies/{id} Get a single movie by ID POST /api/movies Create a new movie PUT /api/movies/{id} Update an existing movie DELETE /api/movies/{id} Delete a movie Models Movie public class Movie { public Guid Id { get; set; }

[Required, StringLength(100)]
public string Title { get; set; } = string.Empty;

[Required, StringLength(100)]
public string Genre { get; set; } = string.Empty;

[Range(0, 3000)]
public int Year { get; set; }
} public record CreateMovieDto { [Required, StringLength(100)] public string Title { get; init; } = string.Empty;

[Required, StringLength(100)]
public string Genre { get; init; } = string.Empty;

[Range(0, 3000)]
public int Year { get; init; }
} public record UpdateMovieDto { [Required, StringLength(100)] public string Title { get; init; } = string.Empty;

[Required, StringLength(100)]
public string Genre { get; init; } = string.Empty;

[Range(0, 3000)]
public int Year { get; init; }
} Running the project Clone the repository git clone https://github.com/your-username/newCRUD.git cd newCRUD Restore dependencies dotnet restore Run the API dotnet run Open in Postman http://localhost:5000/api/movies


#  Subscriptions API

This module provides CRUD operations and advanced querying for managing Subscriptions. It supports pagination, filtering, sorting, and structured responses, following RESTful conventions.
## Class Structure – Subscriptions Module
### 📦 Domain Model
#### `Subscription.cs`
```csharp
public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public int DurationInDays { get; set; }
}

## 🔗 Base Route

/api/subscriptions

## 📌 Endpoints Overview

| Method | Endpoint                     | Requires Body | Body Example / Notes                                                                 |
|--------|------------------------------|---------------|--------------------------------------------------------------------------------------|
| GET    | `/api/subscriptions`         | ❌            | Use query parameters for filtering, pagination, and sorting                         |
| GET    | `/api/subscriptions/{id}`    | ❌            | No body required. `{id}` must be provided in the URL                                |
| POST   | `/api/subscriptions`         | ✅            | `{ "name": "Premium Plan", "price": 29.99, "isActive": true }`                      |
| PUT    | `/api/subscriptions/{id}`    | ✅            | `{ "name": "Premium Plus", "price": 39.99, "isActive": true }` – `{id}` in URL      |
| DELETE | `/api/subscriptions/{id}`    | ❌            | No body required. `{id}` must be provided in the URL                                |

## 🔍 Query Parameters (GET `/api/subscriptions`)

| Parameter     | Type     | Description                                | Example                     |
|---------------|----------|--------------------------------------------|-----------------------------|
| `page`        | int      | Page number (starts at 1)                  | `page=2`                    |
| `pageSize`    | int      | Items per page                             | `pageSize=10`               |
| `sortBy`      | string   | Field to sort by                           | `sortBy=name`               |
| `sortOrder`   | string   | `asc` or `desc`                            | `sortOrder=desc`            |
| `name`        | string   | Filter by name (partial match)             | `name=Premium`              |
| `isActive`    | boolean  | Filter by active status                    | `isActive=true`             |
| `minDuration` | int      | Filter subscriptions with duration ≥ value | `minDuration=30`            |

