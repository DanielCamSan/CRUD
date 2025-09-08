# Cinema API – Team07

API REST desarrollada con **ASP.NET Core** para gestionar **Movies**, **Users** y **Subscriptions**.  

Cada recurso implementa un **CRUD completo** con:
- Persistencia en memoria (`List<T>`)
- Identificadores **GUID**
- DTOs con validaciones (`DataAnnotations`)
- Endpoints con **paginación, filtros y ordenamiento**
- Respuesta uniforme `{ data, meta }`

---

## Requisitos

- .NET SDK 8 o 9  
- Visual Studio 2022 o VS Code  
- Navegador web (para probar con Swagger)  

---

##  Cómo ejecutar

1. Clonar el repositorio.  
2. En la raíz del proyecto, ejecutar:

   ```bash
   dotnet build
   dotnet run
   ```

3. Abrir en el navegador:

   ```
   https://localhost:<puerto>/swagger
   ```

---

##  Estructura del proyecto

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

##  Endpoints principales

### Movies
- `GET /api/v1/movies?page=1&limit=5&sort=year&order=desc&q=action`
- `GET /api/v1/movies/{id}`
- `POST /api/v1/movies`
- `PUT /api/v1/movies/{id}`
- `DELETE /api/v1/movies/{id}`

**Ejemplo POST**
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

**Ejemplo POST**
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

**Ejemplo POST**
```json
{
  "subscriptionDate": "2025-09-05T00:00:00Z",
  "duration": 6,
  "name": "Plan Estudiante"
}
```

---

##  Ejemplo de respuesta de listados

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

## Validaciones

- `[Required]` → campos obligatorios  
- `[StringLength]` → límite de caracteres  
- `[Range]` → valores mínimos y máximos  
- `[ApiController]` → validación automática de ModelState (400 en errores)  

---

##  Flujo de ramas

- **Rama del equipo**: `cinema/team07`  
- **Subramas**:  
  - `cinema/team07-movies`  
  - `cinema/team07-users`  
  - `cinema/team07-subscriptions`  

**Pull Requests**:  
1. Cada subrama → `cinema/team07`  
2. Rama del equipo (`cinema/team07`) → `main`  

---

##  Checklist de entrega

-  Movies CRUD con filtros, paginación y ordenamiento  
-  Users CRUD   
-  Subscriptions CRUD con filtros, paginación y ordenamiento  
-  Respuesta uniforme `{ data, meta }` en listados  
-  Validaciones con `DataAnnotations`  
-  Swagger activo  
-  Rama `cinema/team07` integrada con todas las subramas  

