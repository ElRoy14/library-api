# Library API

API REST en .NET 8 para gestionar libros y autores. Funciona como proxy de [FakeRestAPI](https://fakerestapi.azurewebsites.net/index.html): no usa base de datos, todas las operaciones se delegan a esa API externa.

El frontend que la consume está en [library-ui](https://github.com/ElRoy14/library-ui).

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Cómo ejecutarlo

```bash
git clone https://github.com/ElRoy14/library-api.git
cd library-api
dotnet dev-certs https --trust
dotnet run --project Api --launch-profile https
```

La API queda en `https://localhost:7100` y Swagger en `https://localhost:7100/swagger`.

También se puede abrir `Library.sln` en Visual Studio 2022 y ejecutar el perfil `https`.

## Tests

```bash
dotnet test
```

Cubren los servicios de la capa Application, con xUnit y NSubstitute.

## Configuración

En `Api/appsettings.json`:

| Clave | Descripción | Valor por defecto |
|---|---|---|
| `FakeRestApi:BaseUrl` | URL base de la API externa | `https://fakerestapi.azurewebsites.net/api/v1/` |
| `Cors:AllowedOrigins` | Orígenes permitidos | `http://localhost:4200` |

La URL base se valida al arrancar. Si falta o no es válida, la aplicación no inicia.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/books` | Lista de libros |
| GET | `/api/books/{id}` | Libro por id |
| GET | `/api/books/{id}/details` | Libro con sus autores |
| POST | `/api/books` | Crea un libro |
| PUT | `/api/books/{id}` | Actualiza un libro |
| DELETE | `/api/books/{id}` | Elimina un libro |
| GET | `/api/authors` | Lista de autores con su número de libros |
| GET | `/api/authors/{id}` | Autor por id |
| POST | `/api/authors` | Crea un autor |
| PUT | `/api/authors/{id}` | Actualiza un autor |
| DELETE | `/api/authors/{id}` | Elimina un autor |

`/api/books/{id}/details` no está en el enunciado. Lo agregué para la pantalla de detalle, que necesita el libro junto con su autor sin que el frontend tenga que cruzar dos listas.

Los errores se devuelven en formato ProblemDetails (RFC 7807):

- **400**: datos de entrada inválidos.
- **404**: el libro o autor no existe.
- **502**: FakeRestAPI respondió con error.
- **504**: FakeRestAPI no respondió a tiempo.

## Arquitectura

Clean Architecture en cuatro proyectos, con las dependencias apuntando hacia el dominio:

- **Domain**: entidades `Book` y `Author`. No depende de ningún otro proyecto.
- **Application**: servicios, DTOs, validaciones e interfaces de los repositorios.
- **Infrastructure**: repositorios que consumen FakeRestAPI con `HttpClient`.
- **Api**: controllers, manejo global de errores y configuración.