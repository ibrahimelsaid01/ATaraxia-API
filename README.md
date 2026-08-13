# ATaraxia API

ATaraxia is an ASP.NET Core Web API focused on managing wellness-oriented digital content, user authentication, role-based administration, and Stripe payment integration.

The project is structured as a multi-project .NET solution with separated Core, data-access/infrastructure, and API layers. It demonstrates practical usage of ASP.NET Core Identity, JWT authentication, refresh tokens, Entity Framework Core, SQL Server, Repository and Unit of Work patterns, and protected payment endpoints.

---

## Features

### Authentication & Authorization

- User registration using ASP.NET Core Identity
- User login with JWT access tokens
- Refresh token generation and rotation
- Refresh tokens stored using HTTP-only secure cookies
- Refresh token revocation
- Role-based authorization
- Admin role support
- Admin-only role assignment
- Automatic Admin role/user seeding through configuration
- Account lockout-aware password verification

### Template Management

ATaraxia manages wellness-related digital content templates.

The API supports:

- Retrieve all templates
- Retrieve a template by ID
- Search for a template by name
- Create templates
- Update templates
- Delete templates
- Template and user-like relationships

Template modification endpoints are restricted to users with the `Admin` role.

### Stripe Integration

The project includes authenticated Stripe endpoints for:

- Creating Stripe customers
- Creating payments
- Processing payment information through the Stripe .NET SDK

> **Note:** The current Stripe implementation is intended for development and demonstration purposes. A production payment system should use a modern client-side payment flow such as Stripe Payment Element / Stripe.js with Payment Intents and server-side payment amount validation.

### Data Persistence

- SQL Server
- Entity Framework Core
- Code First migrations
- ASP.NET Core Identity tables
- Refresh token persistence
- Template and user-like relationships

---

## Technology Stack

| Technology | Usage |
|---|---|
| .NET 10 | Application runtime |
| ASP.NET Core Web API | REST API development |
| Entity Framework Core 10 | ORM and data access |
| SQL Server | Relational database |
| ASP.NET Core Identity | User and role management |
| JWT Bearer Authentication | API authentication |
| Refresh Tokens | Session renewal |
| Stripe.net | Stripe integration |
| Swagger / OpenAPI | API exploration and testing |
| Repository Pattern | Data-access abstraction |
| Unit of Work Pattern | Coordinated database operations |

---

## Solution Architecture

The solution is divided into three projects:

```text
ATaraxia
│
├── ATaraxia.Core
│   ├── Configration
│   ├── Models
│   │   ├── Auth
│   │   ├── Entities
│   │   └── Stripe
│   ├── Repositories
│   └── IUnitOfWork.cs
│
├── ATaraxia.EF
│   ├── Migrations
│   ├── Repositories
│   ├── ApplicationDbContext.cs
│   └── UnitOfWork.cs
│
├── ATaraxiaApi
│   ├── Controllers
│   ├── Properties
│   ├── Program.cs
│   └── appsettings.json
│
├── .gitignore
├── ATaraxia.Api.sln
└── README.md
```

### ATaraxia.Core

Contains the application's core contracts and models, including:

- Authentication models
- Application entities
- Stripe models
- Repository interfaces
- JWT configuration
- Unit of Work interface

### ATaraxia.EF

Contains the persistence and infrastructure implementation:

- `ApplicationDbContext`
- Entity Framework Core migrations
- Repository implementations
- Authentication service
- Stripe service
- Unit of Work implementation

### ATaraxiaApi

The API presentation layer containing:

- API controllers
- Dependency injection configuration
- JWT authentication configuration
- ASP.NET Core Identity configuration
- Swagger configuration
- Admin seeding
- Application startup pipeline

---

## Authentication Flow

### Registration

```text
Register Request
       │
       ▼
ASP.NET Core Identity
       │
       ▼
Create User
       │
       ▼
Assign "User" Role
       │
       ▼
Generate JWT
       │
       ▼
Generate Refresh Token
       │
       ▼
Persist Refresh Token
       │
       ▼
Return Access Token
       +
HTTP-only Refresh Cookie
```

### Login

After successful credential validation:

1. A JWT access token is generated.
2. The user's roles are retrieved.
3. An active refresh token is reused when available.
4. Otherwise, a new refresh token is generated and persisted.
5. The refresh token is sent using an HTTP-only secure cookie.

### Refresh Token Rotation

```text
Existing Refresh Token
        │
        ▼
Validate Token
        │
        ▼
Revoke Old Token
        │
        ▼
Generate New Refresh Token
        │
        ▼
Persist New Token
        │
        ▼
Generate New JWT
```

---

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Auth/register` | Register a new user |
| `POST` | `/api/Auth/Login` | Authenticate a user |
| `POST` | `/api/Auth/refreshToken` | Generate new access and refresh tokens |
| `POST` | `/api/Auth/revokeToken` | Revoke a refresh token |
| `POST` | `/api/Auth/addrole` | Assign a role to a user — Admin only |

### Templates

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Template/GetItems` | Retrieve all templates |
| `GET` | `/api/Template/GetItem?id={id}` | Retrieve a template by ID |
| `GET` | `/api/Template/GetByName?name={name}` | Find a template by title |
| `POST` | `/api/Template` | Create a template — Admin only |
| `PUT` | `/api/Template/{id}` | Update a template — Admin only |
| `DELETE` | `/api/Template/{id}` | Delete a template — Admin only |

### Stripe

Stripe endpoints require authentication.

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Stripe/customer/add` | Create a Stripe customer |
| `POST` | `/api/Stripe/payment/add` | Create a Stripe payment |

---

## Security

The project includes:

- JWT Bearer authentication
- ASP.NET Core Identity
- Role-based authorization
- Admin-only protected operations
- HTTP-only refresh token cookies
- Secure refresh token cookies
- Refresh token expiration
- Refresh token revocation
- Refresh token rotation
- JWT lifetime validation
- Zero JWT clock skew
- Lockout-aware login verification
- Sensitive credentials excluded from source control
- Development secrets stored using .NET User Secrets

Sensitive configuration such as JWT signing keys, Stripe secret keys, and seeded Admin credentials must **never be committed to Git**.

---

## Prerequisites

Before running the project, make sure you have:

- .NET 10 SDK
- SQL Server
- Visual Studio or another compatible IDE
- EF Core CLI tools if migrations will be executed
- A Stripe test account if Stripe endpoints will be tested

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/ibrahimelsaid01/ATaraxia-API.git
cd ATaraxia-API
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure the Database

The default development configuration uses SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ATaraxia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

Update the connection string if your SQL Server environment is different.

For production or remote database credentials, use secure configuration rather than committing credentials to the repository.

---

## Configure Development Secrets

The API project uses .NET User Secrets.

Navigate to the API project:

```bash
cd ATaraxiaApi
```

Set a secure JWT signing key:

```bash
dotnet user-secrets set "JWT:Key" "YOUR_SECURE_RANDOM_JWT_KEY"
```

Set your Stripe test secret key:

```bash
dotnet user-secrets set "StripeSettings:SecretKey" "YOUR_STRIPE_TEST_SECRET_KEY"
```

Optional Admin seeding:

```bash
dotnet user-secrets set "AdminSeed:Email" "admin@example.com"
dotnet user-secrets set "AdminSeed:Password" "YOUR_SECURE_ADMIN_PASSWORD"
```

Do not place real secret values in the repository.

---

## Apply Database Migrations

From the solution directory:

```bash
dotnet ef database update --project ATaraxia.EF --startup-project ATaraxiaApi
```

If the EF Core CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## Run the API

From the solution directory:

```bash
dotnet run --project ATaraxiaApi
```

Swagger is enabled in the Development environment and can be used to inspect and test the available API endpoints.

---

## Configuration

The committed `appsettings.json` intentionally does not contain sensitive values.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ATaraxia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  },
  "StripeSettings": {
    "SecretKey": ""
  },
  "JWT": {
    "Key": "",
    "Issuer": "SecuerApi",
    "Audience": "https://localhost:7062",
    "DurationInDays": 10
  }
}
```

Development secrets should be supplied using User Secrets or environment variables.

---

## Database Entities

The current data model includes:

- `ApplicationUser`
- `RefreshToken`
- `Template`
- `UserLike`
- `User`
- `Device`
- `Question`

ASP.NET Core Identity also manages its standard users, roles, claims, logins, and authentication-related tables.

---

## Repository & Unit of Work

The project uses repository abstractions for data access.

```text
Controller
    │
    ▼
IUnitOfWork
    │
    ├── ITemplateRepository
    │
    └── IUserRepository
             │
             ▼
    Repository Implementations
             │
             ▼
    ApplicationDbContext
             │
             ▼
         SQL Server
```

`UnitOfWork` coordinates repository access and commits pending Entity Framework Core changes through `SaveChangesAsync()`.

---

## Build Status

The reviewed version builds successfully with:

```text
0 Errors
0 Warnings
```

Target framework:

```text
.NET 10
```

---

## Future Improvements

Potential improvements for future versions include:

- Migrating Stripe integration to Payment Intents and Stripe Payment Element
- Shortening JWT access-token lifetime while retaining refresh-token sessions
- Adding API rate limiting
- Adding centralized exception handling
- Introducing dedicated request and response DTOs for additional endpoints
- Adding Unit and Integration tests
- Expanding user, device, question, and recommendation functionality
- Adding dedicated Like / Unlike operations for templates

---

## Author

**Ibrahim Elsaid**

Full-Stack Developer (.NET & Angular)

---

## Project Status

This project is maintained as a portfolio project demonstrating backend development concepts using ASP.NET Core, Entity Framework Core, Identity, JWT authentication, SQL Server, and Stripe integration.
