# Project Overview

## Summary
This project is a backend and frontend system for an Aboriginal Art Gallery of Australia. It allows users to browse artifacts, artists, Aboriginal tribes, exhibitions, comments, and map-based origin data.

The upgraded version adds an `Exhibitions` bounded context and improves the backend design by moving database logic into services.

## Technology Stack
- ASP.NET Core Web API
- PostgreSQL
- Entity Framework Core
- PostGIS
- React
- React Router
- Leaflet and React Leaflet
- Swagger

## Main Features
- API endpoints for tribes, artists, artifacts, comments, and exhibitions
- Service layer for application logic
- Async EF Core database calls
- Soft delete instead of physical delete
- Global query filters to hide deleted records
- Database-level `CreatedAt` defaults using `CURRENT_TIMESTAMP`
- PostGIS location support for tribes and artifacts
- React frontend with routing
- Artifact detail pages with comments
- Public Exhibitions page
- Map page showing tribe and artifact origins
- Manage Gallery page with create forms and delete buttons

## Architecture
The backend now follows this simple flow:

```text
Controller -> Service -> AppDbContext -> PostgreSQL/PostGIS
```

Controllers receive HTTP requests and return responses. Services contain the main data access and business logic. `AppDbContext` connects the service layer to PostgreSQL.

This keeps controllers simpler and makes the project easier to extend.

## UI Role Separation
The frontend uses UI separation only.

Public pages allow visitors to view content and add comments. The `Manage Gallery` page contains manager/admin-style tools for creating and deleting records.

This is not real security. There is no login, password, JWT, authentication, or authorization system in this project.

## Reason for Chosen Scope
The project was designed to meet the task requirements clearly while staying manageable. It includes five bounded contexts, geographical data, a frontend interface, and a simple service-layer design without adding repository classes.
