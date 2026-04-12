# Bounded Contexts

## 1. Aboriginal Tribes
Stores Aboriginal tribe information, cultural origin region, and map location.

Main fields:
- Id
- Name
- Description
- StateOrTerritory
- OriginRegionName
- OriginLocation
- CreatedAt
- UpdatedAt
- IsDeleted
- DeletedAt

Relationships:
- One tribe has many artists.
- One tribe has many artifacts.

## 2. Artists
Stores artist information and connects each artist to one Aboriginal tribe.

Main fields:
- Id
- FullName
- Biography
- BirthYear
- DeathYear
- Region
- PhotoUrl
- AboriginalTribeId
- CreatedAt
- UpdatedAt
- IsDeleted
- DeletedAt

Relationships:
- One artist belongs to one tribe.
- One artist has many artifacts.

## 3. Artifacts
Stores artwork information and connects each artifact to an artist, a tribe, and optionally an exhibition.

Main fields:
- Id
- Title
- Description
- YearCreated
- ImageUrl
- Material
- ArtType
- ArtStyle
- Era
- OriginPlaceName
- OriginLocation
- ArtistId
- AboriginalTribeId
- ExhibitionId
- CreatedAt
- UpdatedAt
- IsDeleted
- DeletedAt

Relationships:
- One artifact belongs to one artist.
- One artifact belongs to one tribe.
- One artifact can optionally belong to one exhibition.
- One artifact has many comments.

## 4. Comments
Stores visitor comments connected to artifacts.

Main fields:
- Id
- VisitorName
- VisitorEmail
- Content
- Rating
- ArtifactId
- CreatedAt
- UpdatedAt
- IsDeleted
- DeletedAt

Relationships:
- One comment belongs to one artifact.

`UpdatedAt` is nullable. It starts as `null` when a record is created and is set when the record is edited or deleted.

## 5. Exhibitions
Stores exhibition information for gallery displays or events.

Main fields:
- Id
- Name
- Description
- StartDate
- EndDate
- Location
- CreatedAt
- UpdatedAt
- IsDeleted
- DeletedAt

Relationships:
- One exhibition can have many artifacts.
- Each artifact can optionally belong to one exhibition.

## Soft Delete
All bounded contexts use soft delete.

Instead of physically deleting rows from the database, delete actions set:

```csharp
IsDeleted = true;
DeletedAt = DateTime.UtcNow;
```

EF Core global query filters hide soft-deleted records from normal queries.
