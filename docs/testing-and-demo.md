# Testing and Demo Notes

## Backend Testing
The backend was tested through build checks, migrations, Swagger UI, curl, and the React frontend.

Tested features:
- Create tribe
- Create artist
- Create artifact
- Create comment
- Create exhibition
- Get lists and details
- Get related data by tribe and artist
- Link an artifact to an optional exhibition
- Soft delete records
- Global query filters hiding soft-deleted records
- Map data loading
- Frontend API integration

## Backend Commands
```zsh
cd backend/AboriginalArtGallery.Api
dotnet build
dotnet ef migrations add AddExhibitionsAndSoftDelete
dotnet ef database update
dotnet run
```

The build completed with zero errors. A NuGet vulnerability-data warning may appear if the machine cannot reach `https://api.nuget.org/v3/index.json`; this does not indicate a code compile error.

## Frontend Testing
Tested pages:
- Home
- Artifacts
- Artifact Details
- Artists
- Tribes
- Exhibitions
- Comments
- Map
- Manage Gallery

## Frontend Commands
```zsh
cd frontend
npm install
npm run build
npm start
```

The frontend production build completed successfully.

## Demo Flow
1. Show Swagger and explain the API structure.
2. Show PostgreSQL/PostGIS support.
3. Show the `Exhibitions` API endpoints.
4. Open frontend home page.
5. Show artifacts, artists, tribes, and exhibitions.
6. Open one artifact details page and show the optional exhibition field.
7. Add a comment from the frontend.
8. Open the map page and show tribe/artifact origin markers.
9. Open the Manage Gallery page.
10. Create a new exhibition.
11. Create or update an artifact linked to an exhibition.
12. Use a manager `Delete` button and explain that the backend performs soft delete.

## UI Separation Note
The Manage Gallery page is for manager/admin-style tasks, but this is only UI separation. It is not authentication or authorization.
