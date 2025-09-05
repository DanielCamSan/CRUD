# User's  API:
An ASP.NET Core Web API for managing user records. This project demonstrates full CRUD functionality using an in-memory data store, with a focus on clarity and simplicity.
##  Features

- Create, Read, Update, Patch, and Delete user records
- Auto-generated GUIDs for user IDs
- Partial updates via HTTP PATCH
- RESTful routing with attribute-based controllers
- In-memory data storage using `List<User>`

---

##  User Model

```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```
Method  | Endpoint              | Description
--------|-----------------------|----------------------------------
GET     | /api/users            | Retrieve all users
GET     | /api/users/{id}       | Retrieve a specific user by ID
POST    | /api/users            | Create a new user
PUT     | /api/users/{id}       | Replace an existing user
PATCH   | /api/users/{id}       | Partially update a user
DELETE  | /api/users/{id}       | Delete a user
## Endpoint Requirements Overview
Method  | Endpoint              | Requires Body | Body Format
--------|-----------------------|---------------|--------------------------------------------------
GET     | /api/users            | No            | —
GET     | /api/users/{id}       | No            | —
POST    | /api/users            | Yes           | { "name": "John Doe", "email": "john@example.com" }
PUT     | /api/users/{id}       | Yes           | {"name": "Updated Name", "email": "updated@example.com"}
PATCH   | /api/users/{id}       | Yes           | {"name": "Optional New Name", "email": "Optional New Email"}
DELETE  | /api/users/{id}       | No            | —
## Notes
- User ID (Id) is automatically generated as a Guid when a new user is created via POST. Clients should not provide it manually.
- Data is stored in-memory using List<User>, meaning all user records are lost when the application restarts.
- PUT requests must include both Name and Email. The entire user object is replaced.
- PATCH requests only update fields that are explicitly provided and non-empty:
        Name must be a non-empty string.
        Email must be a non-empty string.
- GET and DELETE endpoints do not require a request body. They operate solely based on the id parameter in the URL.


# Loan Management API:

A minimalistic ASP.NET Core Web API for managing book loans. Built with clarity and modularity in mind, this project demonstrates full CRUD functionality using an in-memory data store.

## Features:

- Create, Read, Update, Patch, and Delete loan records
- Auto-incrementing loan IDs
- UTC-based loan date tracking
- Partial updates via HTTP PATCH
- Clean separation of concerns via Models and Controllers

## Project Structure:

newCRUD/ ├── Controllers/ │   └── LoansController.cs ├── Models/ │   └── Loan.cs

## Loan Model:

```csharp
public class Loan
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
}
```

## API Endpoints

| Method | Endpoint              | Description                    |
|--------|-----------------------|--------------------------------|
| GET    | `/api/loans`          | Retrieve all loans             |
| GET    | `/api/loans/{id}`     | Retrieve a specific loan by ID |
| POST   | `/api/loans`          | Create a new loan              |
| PUT    | `/api/loans/{id}`     | Replace an existing loan       |
| PATCH  | `/api/loans/{id}`     | Partially update a loan        |
| DELETE | `/api/loans/{id}`     | Delete a loan                  |

## Notes

- `LoanDate` is automatically set to `DateTime.UtcNow` when a loan is created.
- `UserId` must be a valid, non-empty `Guid`. Empty GUIDs are ignored in PATCH requests.
- PATCH requests only update fields that are explicitly provided and valid:
  - `BookTitle` must be a non-empty string.
  - `UserId` must be a non-empty GUID.
  - `ReturnDate` must be a valid `DateTime`.
-  PUT requests replace the entire loan object. Be sure to include all fields.
-  DELETE removes the loan by ID. If no match is found, it returns `404 Not Found`.
-  Data is stored in-memory (`List<Loan>`), so all records are lost when the app restarts.

## Getting Started

Follow these steps to run the Loan Management API locally:

- 1. Clone the Repository
- 2. Run the project using visual studio or dotnet run
- 3. Use Postman, curl, or any HTTP client to interact with the API







