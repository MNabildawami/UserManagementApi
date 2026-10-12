# User Management API (ASP.NET Core)

A learning project demonstrating CRUD endpoints, model validation, dependency injection, request logging, and global exception handling in ASP.NET Core Web API.

## Requirements

- .NET 8 SDK or later
- An HTTP client such as curl, Postman, or the REST Client extension for VS Code

## Run locally

```bash
dotnet restore
dotnet run
```

The launch profile listens on `http://localhost:5080`.

## Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/users` | List all users |
| GET | `/api/users/{id}` | Get one user |
| POST | `/api/users` | Create a user |
| PUT | `/api/users/{id}` | Update a user |
| DELETE | `/api/users/{id}` | Delete a user |

### Example POST body

```json
{
  "fullName": "Nabila Putri",
  "email": "nabila@example.com",
  "age": 25
}
```

Validation rules: `fullName` is required and must contain 2–100 characters; `email` must be valid; `age` must be between 1 and 120. Email addresses must be unique.

## Features

- CRUD API endpoints returning conventional HTTP status codes.
- Request validation using data annotations and `[ApiController]`.
- Dependency injection for the `IUserStore` service.
- Request logging middleware that logs method, path, status, and duration.
- Global exception-handling middleware that logs unhandled errors and returns a generic JSON error message.
- In-memory storage so the project can run without a database.

## Limitations

User records are stored in memory and are cleared whenever the app restarts. This project is for learning and review, not production use. A production system should use persistent storage, authentication/authorization, rate limiting, stronger concurrency guarantees, and appropriate secrets management.

## Suggested manual checks

1. `GET /api/users` should initially return an empty array.
2. `POST /api/users` with the example JSON should return `201 Created` and a new ID.
3. `GET /api/users/{id}` should return the created user.
4. Send a duplicate email or invalid age to verify conflict/validation handling.
5. `PUT /api/users/{id}` should update an existing user.
6. `DELETE /api/users/{id}` should return `204 No Content`; requesting the same ID afterward should return `404 Not Found`.

## Copilot use summary template

Replace this with an accurate description of how you actually used Microsoft Copilot during your work. For example: “I used Copilot to draft the initial controller and middleware structure, asked it to explain and troubleshoot compile/runtime issues, and reviewed its suggestions before testing CRUD and validation behavior manually.”
