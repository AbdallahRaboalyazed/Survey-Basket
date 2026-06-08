# Survey Basket API

Survey Basket is a RESTful API for creating, publishing, answering, and analyzing online surveys and polls.

The project focuses on separating the survey lifecycle into clear backend workflows: admins manage polls and questions, members submit votes, and authorized users can view aggregated results. The goal is not only to expose CRUD endpoints, but to keep business rules, authorization, validation, and reporting logic organized enough to evolve without rewriting controller code.

## What problem this project solves

Many survey systems mix management actions, voting actions, and reporting logic in the same flow. This makes the API harder to secure and harder to change later.

This API separates those responsibilities:

- Poll management for creating, updating, publishing, and deleting polls.
- Voting flow for members who answer available polls.
- Results flow for reading vote summaries and analytics.
- Authentication and authorization so each user type only reaches the actions they are allowed to perform.
- Background jobs for scheduled poll notifications.

## Main features

- JWT authentication with refresh token support.
- User registration, login, email confirmation, password reset, and token revocation.
- Role-based and permission-based authorization.
- Poll CRUD operations with publish/unpublish support.
- Question and vote workflow for members.
- Result endpoints for poll analytics, including votes per day and votes per question.
- Input validation using FluentValidation.
- Structured error handling using a Result pattern and Problem Details responses.
- Entity Framework Core with SQL Server.
- Background processing with Hangfire.
- Structured logging with Serilog.
- Health checks endpoint.
- Swagger/OpenAPI support in development.

## Tech stack

- ASP.NET Core Web API (.NET 9)
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- FluentValidation
- Mapster
- Hangfire
- Serilog
- Swagger / OpenAPI
- Health Checks

## Architecture overview

The API is organized around clear backend responsibilities:

```text
Controllers
  -> receive HTTP requests
  -> validate authorization
  -> call services

Services
  -> contain business logic
  -> handle poll, vote, user, role, and result workflows

Persistence
  -> contains database context and EF Core configuration

Contracts
  -> request and response DTOs
  -> keeps API contracts separate from database entities

Entity
  -> domain/database models

Errors
  -> reusable application errors

Authentication
  -> JWT, roles, permissions, and authorization filters
```

This structure keeps controllers thin and moves business decisions into services, which makes the API easier to test, change, and maintain.

## Main API areas

### Authentication

Handles user access and account workflows.

Examples:

- Login
- Register
- Refresh token
- Revoke refresh token
- Confirm email
- Resend confirmation email
- Forgot password
- Reset password

### Polls

Handles survey management.

Examples:

- Get all polls
- Get current published polls
- Get poll by ID
- Create poll
- Update poll
- Delete poll
- Toggle poll publishing status

### Votes

Handles the member voting workflow.

Examples:

- Get available questions for a poll
- Submit a vote for a poll

### Results

Handles poll analytics.

Examples:

- Get poll votes
- Get votes per day
- Get votes per question

### Users and roles

Handles admin-level identity management.

Examples:

- Create users
- Update users
- Toggle user status
- Unlock users
- Manage roles and permissions

## Design decisions

### DTOs instead of exposing entities directly

Request and response contracts are separated from database entities. This keeps the API contract stable even if the internal database model changes.

### Permission-based authorization

The project uses permissions such as poll, user, role, and result permissions instead of relying only on broad roles. This gives more control over what each user can do.

### Result pattern for errors

Services return success or failure results instead of throwing exceptions for normal business cases. Controllers then convert these results into proper HTTP responses.

### Background jobs with Hangfire

Scheduled jobs are handled outside the request/response cycle. For example, poll notifications can run in the background without slowing down user requests.

## Getting started

### Prerequisites

- .NET 9 SDK
- SQL Server
- Visual Studio 2022 or JetBrains Rider
- Postman or Swagger for testing
- EF Core CLI tools if you want to run migrations from the terminal

### Clone the repository

```bash
git clone https://github.com/AbdallahRaboalyazed/Survey-Basket.git
cd Survey-Basket
```

### Restore packages

```bash
dotnet restore
```

### Configure settings

Update the configuration values in `SurveyBasket.Api/appsettings.json` or use user secrets for sensitive values.

Required values include:

- SQL Server connection string
- Hangfire SQL Server connection string
- JWT key
- Mail settings
- Hangfire dashboard username and password
- Allowed origins

Important: do not commit real secrets to GitHub.

### Run the API

```bash
cd SurveyBasket.Api
dotnet run
```

When the API starts, Swagger should be available in development mode.

## Environment variables / secrets

Recommended secret values:

```text
Jwt:Key
MailSettings:Password
HangfireSettings:Username
HangfireSettings:Password
ConnectionStrings:constr
ConnectionStrings:HangfireConnection
```

## Future improvements

- Add unit and integration tests.
- Add Docker support for easier local setup.
- Add CI/CD pipeline.
- Add more detailed API documentation with request and response examples.
- Add rate limiting for authentication endpoints.
- Add caching for frequently requested poll data.

## Author

Abdallah Rabea  
GitHub: https://github.com/AbdallahRaboalyazed  
LinkedIn: https://www.linkedin.com/in/abdallah-rabea-461651255/
