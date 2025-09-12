# Cinema RestAPI DTOs

## API Details:
### Users
#### 1. Method: GetAll
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/users
##### Example:
- **Request:** GET https://localhost:7162/api/v1/users?page=2&limit=1
- **Response** :
~~~
{
    "data": [
        {
            "id": "cb3cf699-8150-48db-8fdd-ed9858389367",
            "name": "Santiago",
            "age": 20,
            "email": "ghost@gmail.com",
            "password": "fercardenas"
        }
    ],
    "meta": {
        "page": 2,
        "limit": 1,
        "total": 2
    }
}
~~~
---
#### 2. Method: GetOne
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/users/{id}
##### Example:
- **Request:** GET https://localhost:7162/api/v1/users/d9153267-be3b-4f8d-9d15-0b08c19005d4
- **Response** :
~~~
{
    "id": "d9153267-be3b-4f8d-9d15-0b08c19005d4",
    "name": "Sebastian",
    "age": 19,
    "email": "sebas@gmail.com",
    "password": "lubevillas2008"
}
~~~
---
#### 3. Method: Create
- **HTTP Method:** POST
- **Endpoint URL:**  https://localhost:7162/api/v1/users
##### Example:
- **Request:** POST https://localhost:7162/api/v1/users
    - **Body** :
~~~
{
    "name": "Alejandro",
    "age": 20,
    "email": "ale@gmail.com",
    "password": "pass"
}
~~~
- **Response** :
~~~
{
    "id": "5ece2d66-5636-4581-8cb7-f8f26aaaedab",
    "name": "Alejandro",
    "age": 20,
    "email": "ale@gmail.com",
    "password": "pass"
}
~~~
---
#### 4. Method: Update
- **HTTP Method:** PUT
- **Endpoint URL:**  https://localhost:7162/api/v1/users/{id}
##### Example:
- **Request:** PUT https://localhost:7162/api/v1/users/21ffa068-22a3-4fe2-bfd8-640fdb798aa6
    - **Body** :
~~~
{
    "Name":"Santi",
    "Email":"santi@gmail.com",
    "password":"123142"
}
~~~
- **Response** :
~~~
{
    "id": "21ffa068-22a3-4fe2-bfd8-640fdb798aa6",
    "name": "Santi",
    "age": 0,
    "email": "santi@gmail.com",
    "password": "123142"
}
~~~
---
#### 5. Method: Delete
- **HTTP Method:** DELETE
- **Endpoint URL:**  https://localhost:7162/api/v1/users/{id}
##### Example:
- **Request:** DELETE https://localhost:7162/api/v1/users/21ffa068-22a3-4fe2-bfd8-640fdb798aa6
- **Response** : 204 No Content
### Movies
#### 1. Method: GetAll
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/movies
##### Example:
- **Request:** GET https://localhost:7162/api/v1/movies?page=1&limit=3&sort=year&order=desc
- **Response** :
~~~
{
    "data": [
        {
            "id": "d4ce307f-1e72-4ac4-b35a-b0f6337f326a",
            "title": "La La Land",
            "genre": "Musical",
            "year": 2016
        },
        {
            "id": "a66888f1-7086-4006-bf7e-47000426b2f2",
            "title": "Mad Max: Fury Road",
            "genre": "Action",
            "year": 2015
        },
        {
            "id": "b48ab028-3a5d-4ec7-82df-1335ebcac05e",
            "title": "Inception",
            "genre": "Sci-Fi",
            "year": 2010
        }
    ],
    "meta": {
        "page": 1,
        "limit": 3,
        "total": 4
    }
}
~~~
---
#### 2. Method: GetOne
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/movies/{id}
##### Example:
- **Request:** GET https://localhost:7162/api/v1/movies/d4ce307f-1e72-4ac4-b35a-b0f6337f326a
~~~
{
    "id": "d4ce307f-1e72-4ac4-b35a-b0f6337f326a",
    "title": "La La Land",
    "genre": "Musical",
    "year": 2016
}
~~~
---
#### 3. Method: Create
- **HTTP Method:** POST
- **Endpoint URL:**  https://localhost:7162/api/v1/movies
##### Example:
- **Request:** POST https://localhost:7162/api/v1/movies
    - **Body** :
~~~
{
    "title": "Interstellar",
    "genre": "Sci-Fi",
    "year": 2014
}
~~~
- **Response** :
~~~
{
    "id": "14e59a29-96cd-4eeb-b3eb-e8ca43a6f135",
    "title": "Interstellar",
    "genre": "Sci-Fi",
    "year": 2014
}
~~~
---
#### 4. Method: Update
- **HTTP Method:** PUT
- **Endpoint URL:**  https://localhost:7162/api/v1/movies/{id}
##### Example:
- **Request:** PUT https://localhost:7162/api/v1/movies/14e59a29-96cd-4eeb-b3eb-e8ca43a6f135
    - **Body** :
~~~
{
    "title": "Interstellar",
    "genre": "Sci-Fi, Space",
    "year": 2024
}
~~~
- **Response** :
~~~
{
    "id": "14e59a29-96cd-4eeb-b3eb-e8ca43a6f135",
    "title": "Interstellar",
    "genre": "Sci-Fi, Space",
    "year": 2024
}
~~~
---
#### 5. Method: Delete
- **HTTP Method:** DELETE
- **Endpoint URL:**  https://localhost:7162/api/v1/movies/{id}
##### Example:
- **Request:** DELETE https://localhost:7162/api/v1/movies/14e59a29-96cd-4eeb-b3eb-e8ca43a6f135
- **Response** : 204 No Content
### Subscriptions
#### 1. Method: GetAll
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/subscriptions
##### Example:
- **Request:** GET https://localhost:7162/api/v1/subscriptions?page=1&limit=2&sort=Name&order=asc
- **Response** :
~~~
{
    "data": [
        {
            "id": "8b7e6c2f-3f59-4c56-b12e-9ad56b2c7a21",
            "subscription_date": "2025-09-11T21:00:00",
            "duration": 60,
            "name": "Plus"
        },
        {
            "id": "5e2f1c9b-4c33-42a1-8b9f-37fcb5d12f44",
            "subscription_date": "2025-09-11T21:00:00",
            "duration": 120,
            "name": "Pro"
        }
    ],
    "meta": {
        "page": 1,
        "limit": 2,
        "total": 4
    }
}
~~~
---
#### 2. Method: GetOne
- **HTTP Method:** GET
- **Endpoint URL:**  https://localhost:7162/api/v1/subscriptions/{id}
##### Example:
- **Request:** GET https://localhost:7162/api/v1/subscriptions/8b7e6c2f-3f59-4c56-b12e-9ad56b2c7a21
- **Response** :
~~~
{
    "id": "8b7e6c2f-3f59-4c56-b12e-9ad56b2c7a21",
    "subscription_date": "2025-09-11T21:00:00",
    "duration": 60,
    "name": "Plus"
}
~~~
---
#### 3. Method: Create
- **HTTP Method:** POST
- **Endpoint URL:**  https://localhost:7162/api/v1/subscriptions
##### Example:
- **Request:** POST https://localhost:7162/api/v1/subscriptions
    - **Body** :
~~~
{
    "subscription_date": "2025-10-01T00:00:00",
    "duration": 90,
    "name": "Premium"
}
~~~
- **Response** :
~~~
{
    "id": "c1234567-89ab-4cde-f012-3456789abcde",
    "subscription_date": "2025-10-01T00:00:00",
    "duration": 90,
    "name": "Premium"
}
~~~
---
#### 4. Method: Update
- **HTTP Method:** PUT
- **Endpoint URL:**  https://localhost:7162/api/v1/subscriptions/{id}
##### Example:
- **Request:** PUT https://localhost:7162/api/v1/subscriptions/c1234567-89ab-4cde-f012-3456789abcde
    - **Body** :
~~~
{
    "subscription_date": "2025-11-01T00:00:00",
    "duration": 120,
    "name": "Pro Plus"
}
~~~
- **Response** :
~~~
{
    "id": "c1234567-89ab-4cde-f012-3456789abcde",
    "subscription_date": "2025-11-01T00:00:00",
    "duration": 120,
    "name": "Pro Plus"
}
~~~
---
#### 5. Method: Delete
- **HTTP Method:** DELETE
- **Endpoint URL:**  https://localhost:7162/api/v1/subscriptions/{id}
##### Example:
- **Request:** DELETE https://localhost:7162/api/v1/subscriptions/c1234567-89ab-4cde-f012-3456789abcde
- **Response** : 204 No Content







