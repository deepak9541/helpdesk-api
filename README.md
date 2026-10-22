# Helpdesk Ticket Management System — API

A role-based support ticket management system built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**. Employees raise support tickets, Admins assign them to Developers, and Developers resolve them — a simplified version of an internal company helpdesk.

> Frontend (Angular)

---

## Features

- **JWT Authentication** with role-based authorization (Employee, Admin, Developer)
- **Role-based data access** — each role sees only the data relevant to them:
  - Employees see tickets they created
  - Developers see tickets assigned to them
  - Admins see all tickets
- **Ticket lifecycle management** — Open → In Progress → Resolved → Closed, with transitions restricted by role
- **Ticket assignment workflow** — Admins assign open tickets to developers
- **Dashboard stats** — ticket counts by status (Admin only)
- **Repository Pattern + Unit of Work** for clean separation between business logic and data access
- **DTOs** for every request/response — entities are never exposed directly
- **Password hashing** with BCrypt — no plain-text passwords stored

---

## Tech Stack

| Layer | Technology |
|---|---|
| API | ASP.NET Core Web API (.NET 8) |
| ORM | Entity Framework Core (Code-First) |
| Database | SQL Server (local) / SQLite (deployment) |
| Auth | JWT Bearer Authentication |
| Password Hashing | BCrypt.Net |
| API Docs | Swagger / OpenAPI |

---

## Architecture

```
Controller  →  Service  →  Repository (via Unit of Work)  →  DbContext  →  Database
```

- **Controllers** — handle HTTP requests/responses only, no business logic
- **Services** — contain business rules (e.g. status transition rules, ownership checks)
- **Repositories** — handle all database access, built on a generic repository + entity-specific repositories
- **Unit of Work** — coordinates repositories and commits changes in a single transaction
- **DTOs** — define what data enters and leaves the API, keeping entities (and sensitive fields like password hashes) internal

---

## Roles & Permissions

| Action | Employee | Admin | Developer |
|---|:---:|:---:|:---:|
| Create ticket | ✅ | ✅ | ❌ |
| View own/assigned tickets | ✅ (own) | ✅ (all) | ✅ (assigned) |
| Edit ticket | ✅ (own, while Open) | ✅ (any) | ❌ |
| Delete ticket | ❌ | ✅ | ❌ |
| Assign ticket to developer | ❌ | ✅ | ❌ |
| Change status → Resolved | ❌ | ✅ | ✅ (own assigned ticket) |
| Change status → Closed | ❌ | ✅ | ❌ |
| View dashboard stats | ❌ | ✅ | ❌ |

---

## API Endpoints

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/login` | Login, returns JWT token |

### Tickets
| Method | Endpoint | Description | Roles |
|---|---|---|---|
| POST | `/api/tickets` | Create a ticket | Employee |
| GET | `/api/tickets/{id}` | Get ticket details | All (own/assigned/any) |
| PUT | `/api/tickets/{id}` | Edit a ticket | Employee (own, Open), Admin |
| DELETE | `/api/tickets/{id}` | Delete a ticket | Admin |
| PATCH | `/api/tickets/{id}/assign` | Assign to a developer | Admin |
| PATCH | `/api/tickets/{id}/status` | Change ticket status | Admin, Developer |

### Users
| Method | Endpoint | Description | Roles |
|---|---|---|---|
| GET | `/api/users/developers` | List developers (for assign dropdown) | Admin |

### Dashboard
| Method | Endpoint | Description | Roles |
|---|---|---|---|
| GET | `/api/dashboard/stats` | Ticket counts by status | Admin |

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (local) or SQLite
- Visual Studio / VS Code

### Setup

1. Clone the repo
   ```bash
   git clone https://github.com/deepak9541/helpdesk-api.git
   cd helpdesk-api
   ```

2. Copy `appsettings.Example.json` to `appsettings.json` and fill in your own values:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=HelpdeskDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Jwt": {
       "Key": "YOUR_SECRET_KEY_AT_LEAST_32_CHARACTERS",
       "Issuer": "HelpdeskApi",
       "Audience": "HelpdeskClient",
       "ExpiryMinutes": 60
     }
   }
   ```

3. Apply migrations
   ```bash
   dotnet ef database update
   ```

4. Run the API
   ```bash
   dotnet run
   ```

5. Open Swagger UI at `https://localhost:<port>/swagger`

### Test Accounts (seeded)

| Role | Email | Password |
|---|---|---|
| Admin | admin@helpdesk.com | Password@123 |
| Employee | rahul@helpdesk.com | Password@123 |
| Employee | priya@helpdesk.com | Password@123 |
| Developer | amit@helpdesk.com | Password@123 |
| Developer | neha@helpdesk.com | Password@123 |

---

## Project Structure

```
HelpdeskApi/
├── Controllers/      # API endpoints
├── Services/          # Business logic
├── Repositories/       # Data access (Generic + Unit of Work)
├── Entities/          # Database models
├── DTOs/              # Request/response contracts
├── Enums/             # TicketStatus, TicketPriority, UserRole
└── Data/              # DbContext, migrations
```

---

## What This Project Demonstrates

- REST API design with proper HTTP verbs and status codes
- JWT authentication and role-based authorization
- Repository Pattern and Unit of Work for maintainable data access
- DTO-based API contracts (no entity leakage, no password hashes in responses)
- Business rule enforcement in the service layer (ownership checks, status-transition rules)
- EF Core Code-First migrations and relationship configuration (Fluent API)

---

## Future Improvements

- Ticket comments / activity history
- Filtering, search, and pagination on the ticket list
- Email notifications on assignment/status change
- Angular frontend (in progress)

---

## Author

**Deepak Gupta**
[GitHub](https://github.com/deepak9541) · Angular Developer, transitioning to Full-Stack
