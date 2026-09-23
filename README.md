# JobApplication API

A RESTful Job Application API built with ASP.NET Core and Clean Architecture.

The system provides authentication, role-based authorization, job management, job applications, CV uploads, application status management, and centralized error handling.

## Features

* User registration and login
* JWT-based authentication
* Role-based authorization
* Candidate and Recruiter roles
* Recruiter job management
* Candidates can apply for jobs
* CV file upload
* View candidate applications
* Recruiters can view applications for their jobs
* Application status management
* Application cancellation
* Job closing
* Centralized exception handling
* Rate limiting for authentication endpoints
* Consistent API responses using `ApiResponse`

## User Roles

### Candidate

Candidates can:

* Register and log in
* View available jobs
* Apply for jobs
* Upload their CV when applying
* View their applications
* Cancel applications when allowed

### Recruiter

Recruiters can:

* Create jobs
* Update their jobs
* Activate or deactivate jobs
* View job details
* Close their jobs
* View applications submitted to their jobs
* Update application statuses

## Business Rules

### Job Management

* Only recruiters can create and manage jobs.
* A recruiter can only modify their own jobs.
* Jobs are active by default when created.
* A closed job cannot accept new applications.
* Closing a job is permanent.
* Closing a job does not delete existing applications.
* Existing applications can still be reviewed after the job is closed.

### Job Applications

* Only candidates can apply for jobs.
* A candidate can apply to a job only once.
* Candidates can only apply to active jobs.
* Candidates cannot apply to closed jobs.
* A CV is required when applying.
* CV files must use an allowed file extension.
* Maximum CV size is 50 MB.

### Application Cancellation

Candidates can cancel their own applications only when the status is:

* `Applied`
* `UnderReview`

Applications cannot be cancelled after reaching:

* `InterView`
* `Accepted`
* `Rejected`

Cancellation does not delete the application. Instead, its status is changed to `Cancelled` and the cancellation time is recorded.

### Application Status

Application statuses follow explicit transitions:

```text
Applied
   ↓
UnderReview
   ↓
InterView
   ↓
Accepted / Rejected
```

* `Accepted` is a final status.
* `Rejected` is a final status.
* `Cancelled` is a final status.
* Invalid status transitions are rejected.
* Accepted and Rejected applications cannot be changed.
* Cancelled applications cannot be updated.

## Architecture

The project follows Clean Architecture principles and is divided into four main layers:

* **Domain** — Entities, enums, and core business models.
* **Application** — DTOs, service interfaces, repository interfaces, and application logic.
* **Infrastructure** — Entity Framework Core, database access, repositories, Identity, and external implementations.
* **API** — Controllers, middleware, authentication configuration, file storage, and API configuration.

## Technologies

* C#
* ASP.NET Core Web API
* .NET
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* Role-Based Authorization
* ASP.NET Core Rate Limiting
* Scalar API Documentation
* RESTful APIs
* Clean Architecture
* Dependency Injection

## API Endpoints

### Authentication

| Method | Endpoint             | Access |
| ------ | -------------------- | ------ |
| POST   | `/api/Auth/register` | Public |
| POST   | `/api/Auth/login`    | Public |

### Jobs

| Method | Endpoint                   | Access              |
| ------ | -------------------------- | ------------------- |
| POST   | `/api/Jobs`                | Recruiter           |
| PUT    | `/api/Jobs/{jobId}`        | Recruiter           |
| PATCH  | `/api/Jobs/{jobId}/status` | Recruiter           |
| GET    | `/api/Jobs/{jobId}`        | Authenticated Users |
| POST   | `/api/Jobs/{jobId}/close`  | Recruiter           |

### Applications

| Method | Endpoint                                  | Access    |
| ------ | ----------------------------------------- | --------- |
| POST   | `/api/Application`                        | Candidate |
| GET    | `/api/Application/my-applications`        | Candidate |
| DELETE | `/api/Application/{applicationId}`        | Candidate |
| GET    | `/api/Application/job/{jobId}`            | Recruiter |
| PATCH  | `/api/Application/{applicationId}/status` | Recruiter |

## Authentication

The API uses JWT Bearer Authentication.

After successful login, the API returns a JWT containing information used to identify the authenticated user and their role.

Protected endpoints require a valid JWT token.

Role-based authorization is applied using the following roles:

* `Candidate`
* `Recruiter`

## Identity & Account Security

ASP.NET Core Identity is used for user management and authentication.

The application also supports account lockout after repeated failed login attempts.

Authentication endpoints are protected using an `auth` rate-limiting policy.

## CV Upload

Candidates can upload their CV when applying for a job.

Supported file extensions:

```text
.pdf
.doc
.docx
.ppt
.pptx
.xls
.xlsx
```

Maximum file size:

```text
50 MB
```

Uploaded files are stored under the application's `wwwroot/uploads/files` directory.

## Error Handling

The API uses centralized exception handling through middleware.

This provides consistent error responses and handles common application and database errors without duplicating exception-handling logic across controllers.

## API Response

The API uses a consistent response wrapper:

```json
{
  "isSuccess": true,
  "message": "Operation completed successfully.",
  "data": {},
  "errors": []
}
```

This provides a consistent structure for successful and failed API responses.

## Database

The project uses:

* Entity Framework Core
* SQL Server
* ASP.NET Core Identity

Database schema changes are managed through EF Core migrations.

Example:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Configuration

Sensitive configuration values such as JWT settings and database connection strings should not be stored directly in source control.

For local development, ASP.NET Core User Secrets can be used to store sensitive configuration.

Example configuration sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Jwt": {
    "Key": "...",
    "Issuer": "...",
    "Audience": "...",
    "DurationInDays": "..."
  }
}
```

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Ahillah/JobApplication.API.git
```

### 2. Configure the database

Set the SQL Server connection string in your development configuration or User Secrets.

### 3. Configure JWT

Add the required JWT settings to your configuration.

### 4. Apply migrations

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

## API Documentation

The project uses **Scalar** for API documentation and testing.

After running the application, open the Scalar endpoint provided by the application to explore and test the available API endpoints.

## What This Project Demonstrates

This project demonstrates practical implementation of:

* Clean Architecture
* ASP.NET Core Web API
* JWT Authentication
* ASP.NET Core Identity
* Role-Based Authorization
* Entity Framework Core
* SQL Server
* Dependency Injection
* DTO-based API design
* Repository-based data access
* File Upload
* Business Rule Enforcement
* Application Status Workflows
* Centralized Exception Handling
* Rate Limiting
* RESTful API design


