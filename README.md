# API de Libros - Team 06

Endpoints para el manejo de libros en el sistema CRUD.

## Tabla de Endpoints

| Método | Endpoint | Descripción | Parámetros |
|--------|----------|-------------|------------|
| **GET** | `/api/libros` | Obtener todos los libros | - |
| **GET** | `/api/libros/{id}` | Obtener libro por ID | `id` (Guid) |
| **POST** | `/api/libros` | Crear nuevo libro | JSON en body |
| **PUT** | `/api/libros/{id}` | Actualizar libro completo | `id` + JSON en body |
| **PATCH** | `/api/libros/{id}` | Actualizar libro parcial | `id` + JSON en body |
| **DELETE** | `/api/libmos/{id}` | Eliminar libro | `id` (Guid) |

## Ejemplo de Body Request

```json
{
  "titulo": "Nombre del libro",
  "autor": "Nombre del autor", 
  "añoPublicacion": 2023,
  "genero": "Género literario"
}
```

## Ejemplos de uso

### Obtener todos los libros
```http
GET /api/libros
```

### Crear un libro
```http
POST /api/libros
Content-Type: application/json

{
  "titulo": "Cien años de soledad",
  "autor": "Gabriel García Márquez",
  "añoPublicacion": 1967,
  "genero": "Realismo mágico"
}
```

### Modelo de datos
```csharp
public class Libro
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Autor { get; set; } = "";
    public int AñoPublicacion { get; set; }
    public string Genero { get; set; } = "";
}
```
