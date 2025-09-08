# Cinema API – Team07

**Group Members:**  
- SORIA AGUIRRE RENE  
- MAIZ HINOJOSA JUAN PABLO  
- RIMASSA FERNANDEZ ERNESTO DAVID  

This is a REST API made with **ASP.NET Core** to manage **Movies**, **Users**, and **Subscriptions**.

Each part has full **CRUD**:
- Data is saved in memory (`List<T>`)
- Every item has a **GUID Id**
- Validation with **DTOs**
- Endpoints have **pagination, filters and sort**
- Responses use this format: `{ data, meta }`

---
## Project structure
```
CRUD/
 ├─ Controllers/
 │   ├─ MoviesController.cs
 │   ├─ UsersController.cs
 │   └─ SubscriptionsController.cs
 ├─ Models/
 │   ├─ Movie.cs
 │   ├─ User.cs
 │   └─ Subscription.cs
 ├─ Program.cs
 └─ README.md
```
---
## Main endpoints

### Movies
- `GET /api/v1/movies?page=1&limit=5&sort=year&order=desc&q=action`
- `GET /api/v1/movies/{id}`
- `POST /api/v1/movies`
- `PUT /api/v1/movies/{id}`
- `DELETE /api/v1/movies/{id}`

**POST example**
```json
{
  "title": "Inception",
  "genre": "Sci-Fi",
  "year": 2010
}
```
---
### Users
- `GET /api/v1/users?page=1&limit=10&q=juan`
- `GET /api/v1/users/{id}`
- `POST /api/v1/users`
- `PUT /api/v1/users/{id}`
- `DELETE /api/v1/users/{id}`

**POST example**
```json
{
  "name": "Juan Pérez",
  "email": "juanperez@example.com"
}
```
---
### Subscriptions
- `GET /api/v1/subscriptions?page=1&limit=10&minDuration=3&maxDuration=12&fromDate=2025-01-01&toDate=2025-12-31`
- `GET /api/v1/subscriptions/{id}`
- `POST /api/v1/subscriptions`
- `PUT /api/v1/subscriptions/{id}`
- `DELETE /api/v1/subscriptions/{id}`

**POST example**
```json
{
  "subscriptionDate": "2025-09-05T00:00:00Z",
  "duration": 6,
  "name": "Plan Estudiante"
}
```

---
Example of list response
```json
{
  "data": [
    {
      "id": "guid",
      "name": "Plan Básico",
      "duration": 6,
      "subscriptionDate": "2025-09-05T00:00:00Z"
    }
  ],
  "meta": {
    "page": 1,
    "limit": 10,
    "total": 3,
    "sort": "SubscriptionDate",
    "order": "desc",
    "q": null,
    "filters": {
      "minDuration": null,
      "maxDuration": null,
      "fromDate": null,
      "toDate": null
    }
  }
}
```
---
## Validations used

- `[Required]` → field is required  
- `[StringLength]` → max characters  
- `[Range]` → min and max values  
- `[ApiController]` → auto check model state (400 if error)
---
## Branches

- **Team branch**: `cinema/team07`  
- **Sub branches**:  
  - `cinema/team07-movies`  
  - `cinema/team07-users`  
  - `cinema/team07-subscriptions`  

**Pull Requests:**  
1. Sub branch → `cinema/team07`  
2. Team branch → `main`  
---
## Delivery checklist

- Movies CRUD with filters, sort and pagination  
- Users CRUD  
- Subscriptions CRUD with filters, sort and pagination  
- Responses use `{ data, meta }` format  
- Validations using `DataAnnotations`  
- Swagger enabled  
- Final branch is `cinema/team07` with all work merged  
