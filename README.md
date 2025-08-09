# EventFlow

A scalable and maintainable **.NET 8** application for managing users, roles, events, and registrations.  
Built with **Clean Architecture** principles to ensure separation of concerns, testability, and maintainability.

---

## Features

- **User Management** – Register, authenticate, and manage users.  
- **Role Management** – Assign and remove roles (Admin, User, Organizer, Attendee).  
- **Event Management** – Create and manage events with participant registration.  
- **Registration** – Register and manage event participation.  
- **Authentication** – JWT-based authentication and role-based authorization.  
- **Error Handling** – Centralized and consistent error responses.  
- **API Documentation** – Integrated Swagger UI for easy API exploration.

---

## Technologies

- **.NET 8**  
- **ASP.NET Core Web API**  
- **Entity Framework Core**  
- **MediatR** (CQRS and request/response handling)  
- **JWT Authentication**  
- **SQL Server**

---

## Architecture

This project follows **Clean Architecture** principles:

- **Domain** – Core business logic and entities.  
- **Application** – Use cases, commands, queries, and interfaces.  
- **Infrastructure** – Data access, external services, and persistence.  
- **API** – HTTP endpoints and middleware.

---



## Project Structure

- `src/EventManagement.Api` – ASP.NET Core Web API (entry point)  
- `src/EventManagement.Application` – Application logic, CQRS, validation  
- `src/EventManagement.Infrastructure` – Data access, security, persistence  
- `src/EventManagement.Domain` – Domain entities, enums, and interfaces  
---

## Design Patterns

### Implemented Patterns

1. **CQRS (Command Query Responsibility Segregation):**  
   - Clean separation between commands (write operations) and queries (read operations)  
   - Each operation has its own handler, validator, and command/query object  
   - Excellent use of MediatR for decoupling  

2. **Repository Pattern with Unit of Work:**  
   - Generic repository base class with common operations  
   - Specific repositories for domain entities  
   - Unit of Work pattern ensures transaction consistency  

3. **Result Pattern:**  
   - Comprehensive error handling without exceptions  
   - Type-safe success/failure results  
   - Implicit conversions for ease of use  

4. **Mediator Pattern:**  
   - MediatR implementation reduces coupling between controllers and business logic  
   - Pipeline behaviors for cross-cutting concerns (validation)  

5. **Validation Pipeline:**  
   - FluentValidation integrated with MediatR pipeline  
   - Automatic validation before command/query execution  

### Potential Pattern Enhancements

- Strategy Pattern for different event notification strategies  
- Factory Pattern for creating different types of events or registration workflows  
- Specification Pattern for complex query filtering logic  
- Domain Events for handling side effects of domain operations  

---

## Default Roles

Seeded roles:  
- Admin  
- Organizer  
- Attendee  
- User

---

## Future Improvements

- **Rich Domain Models** – Add more behavior and rules directly in domain entities.  
- **Security** – Harden security by adding refresh tokens, rate limiting, and audit logging.  
- **Testing** – Increase automated testing coverage, including integration and end-to-end tests.  
- **Docker & CI/CD** – Containerize the application and set up automated pipelines for building, testing, and deployment.
