# API Summary

## DTO Naming
The project uses DTOs to separate API data from database models.

Request DTOs include:
- `TribeCreateRequestDto`
- `TribeUpdateRequestDto`
- `ArtistCreateRequestDto`
- `ArtistUpdateRequestDto`
- `ArtifactCreateRequestDto`
- `ArtifactUpdateRequestDto`
- `CommentCreateRequestDto`
- `CommentUpdateRequestDto`
- `ExhibitionCreateRequestDto`
- `ExhibitionUpdateRequestDto`

Response DTOs include:
- `TribeResponseDto`
- `ArtistResponseDto`
- `ArtifactResponseDto`
- `CommentResponseDto`
- `ExhibitionResponseDto`

Artifact and tribe DTOs expose latitude and longitude instead of raw PostGIS `Point` values.

## Tribe Endpoints
- GET `/api/Tribes`
- GET `/api/Tribes/{id}`
- POST `/api/Tribes`
- PUT `/api/Tribes/{id}`
- DELETE `/api/Tribes/{id}`
- GET `/api/Tribes/{id}/artists`
- GET `/api/Tribes/{id}/artifacts`

## Artist Endpoints
- GET `/api/Artists`
- GET `/api/Artists/{id}`
- POST `/api/Artists`
- PUT `/api/Artists/{id}`
- DELETE `/api/Artists/{id}`
- GET `/api/Artists/{id}/artifacts`

## Artifact Endpoints
- GET `/api/Artifacts`
- GET `/api/Artifacts/{id}`
- POST `/api/Artifacts`
- PUT `/api/Artifacts/{id}`
- DELETE `/api/Artifacts/{id}`
- GET `/api/Artifacts/artist/{artistId}`
- GET `/api/Artifacts/tribe/{tribeId}`

Artifacts can include an optional `exhibitionId` in create and update requests.

## Comment Endpoints
- GET `/api/Comments`
- GET `/api/Comments/{id}`
- GET `/api/Comments/artifact/{artifactId}`
- POST `/api/Comments`
- PUT `/api/Comments/{id}`
- DELETE `/api/Comments/{id}`

## Exhibition Endpoints
- GET `/api/Exhibitions`
- GET `/api/Exhibitions/{id}`
- POST `/api/Exhibitions`
- PUT `/api/Exhibitions/{id}`
- DELETE `/api/Exhibitions/{id}`

## Soft Delete
DELETE endpoints use soft delete. They set `IsDeleted` and `DeletedAt` instead of removing rows with `_context.Remove`.

Normal GET endpoints hide soft-deleted rows because the backend uses EF Core global query filters.

## Async and Services
Controllers call service interfaces instead of using `AppDbContext` directly.

The service layer uses async EF Core methods such as:
- `ToListAsync()`
- `FirstOrDefaultAsync()`
- `FindAsync()`
- `AnyAsync()`
- `SaveChangesAsync()`
