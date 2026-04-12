# Database Relationships

## Main Relationships
- One Aboriginal tribe has many artists.
- One Aboriginal tribe has many artifacts.
- One artist has many artifacts.
- One artifact has many comments.
- One exhibition has many artifacts.
- Each artifact can optionally belong to one exhibition.

## Relationship Summary
- AboriginalTribe -> Artists
- AboriginalTribe -> Artifacts
- Artist -> Artifacts
- Artifact -> Comments
- Exhibition -> Artifacts

## Exhibition Relationship
The `Artifact` model has:

```csharp
public int? ExhibitionId { get; set; }
public Exhibition? Exhibition { get; set; }
```

The `Exhibition` model has:

```csharp
public List<Artifact> Artifacts { get; set; } = new();
```

`ExhibitionId` is nullable because an artifact does not always need to belong to an exhibition.

## Delete Behaviour
The project uses soft delete for tribes, artists, artifacts, comments, and exhibitions.

Soft-deleted rows stay in the database, but normal queries hide them using EF Core global query filters:

```csharp
HasQueryFilter(x => !x.IsDeleted)
```

If an exhibition was physically deleted at the database level, the artifact foreign key is configured with `SetNull`, so the artifact can remain in the system.

## Geographical Data
PostGIS is used to store:
- `AboriginalTribe.OriginLocation`
- `Artifact.OriginLocation`

Both use Point geometry with SRID 4326.

The frontend does not receive raw PostGIS `Point` objects. DTOs use:

```csharp
public double? Latitude { get; set; }
public double? Longitude { get; set; }
```

The backend converts latitude/longitude to `Point` when saving, and converts `Point` back to latitude/longitude when returning responses.

## Timestamps
- `CreatedAt` uses a database default value: `CURRENT_TIMESTAMP`.
- `UpdatedAt` starts as `null` and is set by service methods when a record is updated or deleted.
- `DeletedAt` is set by service methods when a record is soft deleted.
