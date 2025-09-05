# CRUD API - Team 06

This API provides endpoints for CRUD operations on Books, Users, and Loans.

## Complete Endpoints Table

### 📚 Books Controller

| Method | Endpoint | Description | Body Request |
|--------|----------|-------------|--------------|
| **GET** | `api/books` | Get all books | - |
| **GET** | `api/books/{id}` | Get one book by ID | - |
| **POST** | `api/books` | Create new book | `{ "title": "string", "author": "string", "year": number, "genre": "string" }` |
| **PUT** | `api/books/{id}` | Update full book | `{ "title": "string", "author": "string", "year": number, "genre": "string" }` |
| **PATCH** | `api/books/{id}` | Update partial book | `{ "title": "string?", "author": "string?", "year": number?, "genre": "string?" }` |
| **DELETE** | `api/books/{id}` | Delete book | - |

### 👥 Users Controller

| Method | Endpoint | Description | Body Request |
|--------|----------|-------------|--------------|
| **GET** | `api/users` | Get all users | - |
| **GET** | `api/users/{id}` | Get one user by ID | - |
| **POST** | `api/users` | Create new user | `{ "name": "string", "age": number }` |
| **PUT** | `api/users/{id}` | Update full user | `{ "name": "string", "age": number }` |
| **PATCH** | `api/users/{id}` | Update partial user | `{ "name": "string?", "age": number? }` |
| **DELETE** | `api/users/{id}` | Delete user | - |

### 📋 Loans Controller

| Method | Endpoint | Description | Body Request |
|--------|----------|-------------|--------------|
| **GET** | `api/loans` | Get all loans | - |
| **GET** | `api/loans/{id}` | Get one loan by ID | - |
| **POST** | `api/loans` | Create new loan | `{ "bookId": "Guid", "userId": "Guid" }` |
| **PUT** | `api/loans/{id}` | Update full loan | `{ "bookId": "Guid", "userId": "Guid", "loanDate": "DateTime", "returnDate": "DateTime?" }` |
| **DELETE** | `api/loans/{id}` | Delete loan | - |

## Data Models

### Book Model
```csharp
public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public string Genre { get; set; } = "";
}
```

### User Model
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

### Loan Model
```csharp
public class Loan
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
```

## Usage Examples

### Create a Book
```http
POST /api/books
Content-Type: application/json

{
  "title": "One Hundred Years of Solitude",
  "author": "Gabriel García Márquez",
  "year": 1967,
  "genre": "Magic realism"
}
```

### Create a User
```http
POST /api/users
Content-Type: application/json

{
  "name": "John Doe",
  "age": 25
}
```

### Create a Loan
```http
POST /api/loans
Content-Type: application/json

{
  "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa7"
}
```

### Get All Books
```http
GET /api/books
```

### Get User by ID
```http
GET /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

## Important Notes

- All IDs are GUID type
- PATCH endpoints update only provided fields
- Loans are created with current date automatically
- ReturnDate field in loans is optional (can be null)

## Development Team
- Team 06 Members
