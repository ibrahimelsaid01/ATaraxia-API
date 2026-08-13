\# ATaraxia API



ATaraxia is an ASP.NET Core Web API focused on managing wellness-oriented digital content, user authentication, role-based administration, and Stripe payment integration.



The project is structured as a multi-project .NET solution with a separated Core layer, data-access/infrastructure layer, and API layer. It demonstrates practical usage of ASP.NET Core Identity, JWT authentication, refresh tokens, Entity Framework Core, SQL Server, Repository and Unit of Work patterns, and protected payment endpoints.



\---



\## Features



\### Authentication \& Authorization



\- User registration using ASP.NET Core Identity

\- User login with JWT access tokens

\- Refresh token generation and rotation

\- Refresh tokens stored using secure HTTP-only cookies

\- Refresh token revocation

\- Role-based authorization

\- Admin role support

\- Admin-only role assignment

\- Automatic Admin role/user seeding through configuration

\- Account lockout-aware password verification



\### Template Management



ATaraxia manages wellness-related content templates such as:



\- Therapy

\- Yoga

\- Stress

\- Movement

\- Meditation

\- Breathe

\- Soundscape

\- Reels



The API supports:



\- Retrieve all templates

\- Retrieve a template by ID

\- Search for a template by name

\- Create templates

\- Update templates

\- Delete templates

\- Template user-like relationships



Template modification endpoints are restricted to users with the `Admin` role.



\### Stripe Integration



The project includes authenticated Stripe endpoints for:



\- Creating Stripe customers

\- Creating payments

\- Processing payment information through the Stripe .NET SDK



> \*\*Note:\*\* The current Stripe implementation is intended for development/demo purposes. A production payment system should use a modern client-side payment flow such as Stripe Payment Element/Stripe.js together with Payment Intents and server-side payment amount validation.



\### Data Persistence



\- SQL Server

\- Entity Framework Core

\- Code First migrations

\- ASP.NET Core Identity tables

\- Refresh tokens stored as owned Identity user data

\- Template and user-like relationships



\---



\## Technology Stack



| Technology | Usage |

|---|---|

| .NET 10 | Application runtime |

| ASP.NET Core Web API | REST API |

| Entity Framework Core 10 | ORM and data access |

| SQL Server | Relational database |

| ASP.NET Core Identity | User and role management |

| JWT Bearer Authentication | API authentication |

| Refresh Tokens | Session renewal |

| Stripe.net | Stripe integration |

| Swagger / OpenAPI | API exploration and testing |

| Repository Pattern | Data-access abstraction |

| Unit of Work Pattern | Coordinated database operations |



\---



\## Solution Architecture



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

└── ATaraxia.Api.sln

```



\### ATaraxia.Core



Contains the application's core contracts and models, including:



\- Authentication models

\- Application entities

\- Stripe models

\- Repository interfaces

\- JWT configuration

\- Unit of Work interface



\### ATaraxia.EF



Contains the persistence and infrastructure implementation:



\- `ApplicationDbContext`

\- Entity Framework Core migrations

\- Repository implementations

\- Authentication service

\- Stripe service

\- Unit of Work implementation



\### ATaraxiaApi



The API presentation layer containing:



\- API controllers

\- Dependency injection configuration

\- JWT authentication configuration

\- Identity configuration

\- Swagger configuration

\- Admin seeding

\- Application startup pipeline



\---



\## Authentication Flow



\### Registration



```text

Register Request

&#x20;     │

&#x20;     ▼

ASP.NET Core Identity

&#x20;     │

&#x20;     ▼

Create User

&#x20;     │

&#x20;     ▼

Assign "User" Role

&#x20;     │

&#x20;     ▼

Generate JWT

&#x20;     │

&#x20;     ▼

Generate Refresh Token

&#x20;     │

&#x20;     ▼

Persist Refresh Token

&#x20;     │

&#x20;     ▼

Return Access Token

\+ HTTP-only Refresh Cookie

```



\### Login



After successful credential validation:



1\. A JWT access token is generated.

2\. The user's roles are included in the authentication response.

3\. An active refresh token is reused when available.

4\. Otherwise, a new refresh token is generated and persisted.

5\. The refresh token is sent using an HTTP-only secure cookie.



\### Refresh Token Rotation



When a valid refresh token is used:



```text

Existing Refresh Token

&#x20;       │

&#x20;       ▼

Validate Token

&#x20;       │

&#x20;       ▼

Revoke Old Token

&#x20;       │

&#x20;       ▼

Generate New Token

&#x20;       │

&#x20;       ▼

Persist New Token

&#x20;       │

&#x20;       ▼

Generate New JWT

```



\---



\## API Endpoints



\### Authentication



| Method | Endpoint | Description |

|---|---|---|

| POST | `/api/Auth/register` | Register a new user |

| POST | `/api/Auth/Login` | Authenticate a user |

| POST | `/api/Auth/refreshToken` | Generate a new access and refresh token |

| POST | `/api/Auth/revokeToken` | Revoke a refresh token |

| POST | `/api/Auth/addrole` | Assign a role to a user — Admin only |



\### Templates



| Method | Endpoint | Description |

|---|---|---|

| GET | `/api/Template/GetItems` | Retrieve all templates |

| GET | `/api/Template/GetItem?id={id}` | Retrieve a template by ID |

| GET | `/api/Template/GetByName?name={name}` | Find a template by title |

| POST | `/api/Template` | Create a template — Admin only |

| PUT | `/api/Template/{id}` | Update a template — Admin only |

| DELETE | `/api/Template/{id}` | Delete a template — Admin only |



\### Stripe



Stripe endpoints require authentication.



| Method | Endpoint | Description |

|---|---|---|

| POST | `/api/Stripe/customer/add` | Create a Stripe customer |

| POST | `/api/Stripe/payment/add` | Create a Stripe payment |



\---



\## Security



The project includes several security measures:



\- JWT Bearer authentication

\- ASP.NET Core Identity

\- Role-based authorization

\- Admin-only protected operations

\- HTTP-only refresh token cookies

\- Secure refresh token cookies

\- Refresh token expiration

\- Refresh token revocation

\- Refresh token rotation

\- JWT lifetime validation

\- Zero JWT clock skew

\- Login lockout-aware password checking

\- Sensitive credentials excluded from source control

\- Secrets stored with .NET User Secrets during development



Sensitive configuration such as JWT signing keys, Stripe secret keys, and seeded Admin credentials must \*\*never be committed to Git\*\*.



\---



\## Prerequisites



Before running the project, make sure you have:



\- .NET 10 SDK

\- SQL Server

\- Visual Studio 2026 or another compatible IDE

\- EF Core CLI tools if migrations will be executed

\- A Stripe test account if Stripe endpoints will be tested



\---



\## Getting Started



\### 1. Clone the Repository



```bash

git clone <YOUR\_REPOSITORY\_URL>

cd ATaraxia

```



\### 2. Restore Dependencies



```bash

dotnet restore

```



\### 3. Configure the Database



The default development configuration uses SQL Server:



```json

"ConnectionStrings": {

&#x20; "DefaultConnection": "Server=.;Database=ATaraxia;Trusted\_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"

}

```



Update the connection string when necessary for your environment.



For sensitive production/database credentials, use secure configuration rather than committing credentials to `appsettings.json`.



\---



\## Configure Development Secrets



The API project uses .NET User Secrets.



Navigate to:



```bash

cd ATaraxiaApi

```



Set a secure JWT signing key:



```bash

dotnet user-secrets set "JWT:Key" "YOUR\_SECURE\_RANDOM\_JWT\_KEY"

```



Set your Stripe test secret:



```bash

dotnet user-secrets set "StripeSettings:SecretKey" "YOUR\_STRIPE\_TEST\_SECRET\_KEY"

```



Optional Admin seeding:



```bash

dotnet user-secrets set "AdminSeed:Email" "admin@example.com"

dotnet user-secrets set "AdminSeed:Password" "YOUR\_SECURE\_ADMIN\_PASSWORD"

```



Do not place real values for these settings in the repository.



\---



\## Apply Database Migrations



From the solution directory:



```bash

dotnet ef database update --project ATaraxia.EF --startup-project ATaraxiaApi

```



If the EF CLI is not installed:



```bash

dotnet tool install --global dotnet-ef

```



\---



\## Run the API



From the solution directory:



```bash

dotnet run --project ATaraxiaApi

```



Swagger is enabled in the Development environment and can be used to inspect and test the available API endpoints.



\---



\## Configuration



The committed `appsettings.json` intentionally does not contain sensitive values.



Example:



```json

{

&#x20; "ConnectionStrings": {

&#x20;   "DefaultConnection": "Server=.;Database=ATaraxia;Trusted\_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"

&#x20; },

&#x20; "StripeSettings": {

&#x20;   "SecretKey": ""

&#x20; },

&#x20; "JWT": {

&#x20;   "Key": "",

&#x20;   "Issuer": "SecuerApi",

&#x20;   "Audience": "https://localhost:7062",

&#x20;   "DurationInDays": 10

&#x20; }

}

```



Development secrets should be supplied using User Secrets or environment variables.



\---



\## Database Entities



The current data model includes:



\- `ApplicationUser`

\- `RefreshToken`

\- `Template`

\- `UserLike`

\- `User`

\- `Device`

\- `Question`



ASP.NET Core Identity also manages its standard users, roles, claims, logins, and authentication-related tables.



\---



\## Repository \& Unit of Work



The project uses repository abstractions for data access.



```text

Controller

&#x20;   │

&#x20;   ▼

IUnitOfWork

&#x20;   │

&#x20;   ├── ITemplateRepository

&#x20;   └── IUserRepository

&#x20;           │

&#x20;           ▼

&#x20;    Repository Implementations

&#x20;           │

&#x20;           ▼

&#x20;    ApplicationDbContext

&#x20;           │

&#x20;           ▼

&#x20;        SQL Server

```



`UnitOfWork` coordinates repository access and commits pending Entity Framework Core changes through `SaveChangesAsync()`.



\---



\## Build Status



The current reviewed version builds successfully with:



```text

0 Errors

0 Warnings

```



Target framework:



```text

.NET 10

```



\---



\## Development Notes



This repository represents a reviewed and modernized version of the original project.



Areas suitable for future enhancement include:



\- Migrating the Stripe integration to Payment Intents and Stripe Payment Element

\- Shortening JWT access-token lifetime while retaining refresh-token sessions

\- Adding API rate limiting

\- Adding centralized exception handling

\- Introducing dedicated request/response DTOs for additional endpoints

\- Adding Unit and Integration tests

\- Expanding the current user, device, question, and recommendation functionality

\- Adding dedicated Like/Unlike operations for templates



\---



\## Author



\*\*Ibrahim Elsaid\*\*



Full-Stack Developer (.NET \& Angular)



\---



\## Project Status



The project is actively maintained as a portfolio project and demonstrates backend development concepts using the modern ASP.NET Core and .NET ecosystem.

