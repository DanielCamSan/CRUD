# Cinema Team10

This project demonstrates the implementation of CRUD operations with DTOs, validations, and in-memory persistence for a cinema API.

---

## Team Members

- **Member 1:** ELIAS SORIA JOAQUIN MATEO  
- **Member 2:** GARCIA MEZA OLMOS FABIO ADRIAN  
- **Member 3:** MONTAÑO MEJIA KATHERINE FABIANA  
- **Member 4:** PITA VARGAS ARIANA AYLEN  

---

## Objective

Implement CRUD for the following entities using DTOs and validations:

- Movies
- Users  
- Subscriptions  

All data is stored in-memory for demonstration purposes.

---

## Features

- Movies: Create, Read, Update, Delete movie records.  
- Users: Manage cinema users (CRUD).  
- Subscriptions: Manage subscription plans for users (CRUD).  
- DTOs (Data Transfer Objects): Decouple API requests/responses from entities.  
- Validations: Ensure data integrity (required fields, correct formats, ranges).  
- Pagination, filtering, and sorting: All GET endpoints support pagination, filtering, sorting, and meta info in the response.

---

## Tech Stack 
- DataAnnotations (for validations)  
- Postman (for testing endpoints)  

---

## Endpoints Overview

### Movies
- `GET /api/v1/movies` → List all movies (supports pagination, search, genre filter, sorting)  
- `GET /api/v1/movies/{id}` → Get a movie by ID  
- `POST /api/v1/movies` → Create a new movie  
- `PUT /api/v1/movies/{id}` → Update a movie  
- `DELETE /api/v1/movies/{id}` → Delete a movie  

### Users
- `GET /api/v1/users` → List all users (supports pagination, search, age filter, sorting)  
- `GET /api/v1/users/{id}` → Get user by ID  
- `POST /api/v1/users` → Register a new user  
- `PUT /api/v1/users/{id}` → Update user information  
- `DELETE /api/v1/users/{id}` → Delete a user  

### Subscriptions
- `GET /api/v1/subscriptions` → List all subscriptions (supports pagination, search, date/duration filters, sorting)  
- `GET /api/v1/subscriptions/{id}` → Get subscription by ID  
- `POST /api/v1/subscriptions` → Create a subscription  
- `PUT /api/v1/subscriptions/{id}` → Update subscription  
- `DELETE /api/v1/subscriptions/{id}` → Cancel subscription  

---

## Example Validations

- **Movie:** Must have a title and a release year ≥ 1900  
- **User:** Must have a valid email and non-empty name  
- **Subscription:** Must have valid start and end dates (end date > start date)  

---

## Branches

- `cinema/team10` → Main integration branch for the team


