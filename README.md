# Aboriginal Art Gallery

## Project Overview
This project is a full-stack Aboriginal Art Gallery system built for SIT331 Task 5.2HD. It includes an ASP.NET Core Web API backend, a PostgreSQL database with PostGIS support, and a React frontend.

The upgraded version adds a fifth bounded context, `Exhibitions`, and improves the backend structure by using services, async EF Core methods, DTOs, soft delete, and database-level defaults for `CreatedAt`.

## Technology Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- PostGIS
- Swagger with XML comments

### Frontend
- React
- React Router
- Leaflet
- React Leaflet

## Bounded Contexts
The project includes these bounded contexts:
- Aboriginal Tribes
- Artists
- Artifacts
- Comments
- Exhibitions

## Main Features
- API endpoints for tribes, artists, artifacts, comments, and exhibitions
- Service layer between controllers and `AppDbContext`
- Async database access using EF Core async methods
- DTOs for create requests, update requests, and responses
- PostGIS location support using `Point` geometry for tribe and artifact origin locations
- Latitude and longitude DTO fields so the frontend does not receive raw PostGIS `Point` objects
- Soft delete using `IsDeleted` and `DeletedAt`
- Global EF Core query filters to hide soft-deleted records
- Database-level `CreatedAt` default values using `CURRENT_TIMESTAMP`
- React frontend with routing
- Artifact details page with public comment form
- Exhibitions page
- Interactive map page with tribe and artifact markers
- Manage Gallery page for manager/admin-style actions

## UI Role Separation
The frontend separates public pages from manager tools.

Public users can view home, artifacts, artifact details, artists, tribes, exhibitions, comments, and the map. They can also add comments.

Manager tools are kept on the `Manage Gallery` page. That page includes create forms and delete buttons.

This is only UI separation. The project does not implement real authentication, authorization, passwords, or JWT security.

## Project Structure
```text
aboriginal-art-gallery-2/
  backend/
    AboriginalArtGallery.Api/
      Controllers/
      Data/
      DTOs/
      Interfaces/
      Models/
      Services/
  frontend/
    src/
      Components/
      pages/
      services/
  docs/
  README.md
```

## Backend Commands
```zsh
cd backend/AboriginalArtGallery.Api
dotnet build
dotnet run
```

Swagger is available when the backend is running in development:

```text
http://localhost:5141/swagger
```

## Frontend Commands
```zsh
cd frontend
npm install
npm start
npm run build
```

## Database Migration
The main upgrade migration is:

```text
AddExhibitionsAndSoftDelete
```

It adds the `exhibitions` table, adds `ExhibitionId` to artifacts, adds soft-delete columns, adds nullable `UpdatedAt` timestamps, and configures `CreatedAt` defaults.

The follow-up migration `MakeUpdatedAtNullable` makes `UpdatedAt` nullable for entities that previously required a value on creation.

## Additional Documentation
More project documentation is included in the `docs` folder.
