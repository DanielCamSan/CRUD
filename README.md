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

public class Loan
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
}

## API Endpoints

| Method | Endpoint              | Description                    |
|--------|-----------------------|--------------------------------|
| GET    | `/api/loans`          | Retrieve all loans             |
| GET    | `/api/loans/{id}`     | Retrieve a specific loan by ID |
| POST   | `/api/loans`          | Create a new loan              |
| PUT    | `/api/loans/{id}`     | Replace an existing loan       |
| PATCH  | `/api/loans/{id}`     | Partially update a loan        |
| DELETE | `/api/loans/{id}`     | Delete a loan                  |

## Example Payloads:

### GET All Loans

**Request**
```http
GET /api/loans

**Response**
[
  {
    "id": 1,
    "bookTitle": "Clean Code",
    "userId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
    "loanDate": "2025-09-02T14:00:00Z",
    "returnDate": null
  }
]

### GET Loan by ID

**Request**
GET /api/loans/1

**Response**
{
  "id": 1,
  "bookTitle": "Clean Code",
  "userId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
  "loanDate": "2025-09-02T14:00:00Z",
  "returnDate": null
}

### POST create loan

**Request**
POST /api/loans
Content-Type: application/json

**Body**
{
  "bookTitle": "The Pragmatic Programmer",
  "userId": "a1b2c3d4-e5f6-7890-abcd-1234567890ef"
}

**Response**
{
  "id": 2,
  "bookTitle": "The Pragmatic Programmer",
  "userId": "a1b2c3d4-e5f6-7890-abcd-1234567890ef",
  "loanDate": "2025-09-02T14:05:00Z",
  "returnDate": null
}

### PUT Update Loan

**Request**
PUT /api/loans/2
Content-Type: application/json

**Body**
{
  "bookTitle": "Refactoring",
  "userId": "a1b2c3d4-e5f6-7890-abcd-1234567890ef",
  "loanDate": "2025-09-02T14:05:00Z",
  "returnDate": "2025-09-10T10:00:00Z"
}

**Response**
{
  "id": 2,
  "bookTitle": "Refactoring",
  "userId": "a1b2c3d4-e5f6-7890-abcd-1234567890ef",
  "loanDate": "2025-09-02T14:05:00Z",
  "returnDate": "2025-09-10T10:00:00Z"
}

### PATCH Partial Update


**Request**
PATCH /api/loans/2
Content-Type: application/json

**Body**
{
  "returnDate": "2025-09-12T09:00:00Z"
}

**Response**
{
  "id": 2,
  "bookTitle": "Refactoring",
  "userId": "a1b2c3d4-e5f6-7890-abcd-1234567890ef",
  "loanDate": "2025-09-02T14:05:00Z",
  "returnDate": "2025-09-12T09:00:00Z"
}

### DELETE Loan

**Request**
DELETE /api/loans/2

**Response**
204 No Content

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







