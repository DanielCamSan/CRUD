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




