# Aboriginal Art Gallery Frontend

This React frontend is part of the SIT331 Aboriginal Art Gallery project.

## Pages
- Home
- Artifacts
- Artifact Details
- Artists
- Tribes
- Exhibitions
- Comments
- Map
- Manage Gallery

## Public Pages
Public users can:
- view home
- view artifacts
- view artifact details
- add comments
- view artists
- view tribes
- view exhibitions
- view the map

## Manage Gallery
The `Manage Gallery` page contains manager/admin-style tools:
- create tribes
- create artists
- create artifacts
- create exhibitions
- delete records

The delete buttons call backend DELETE endpoints. The backend performs soft delete by setting `IsDeleted` and `DeletedAt`.

This is UI separation only. There is no real authentication, authorization, password system, or JWT security in this frontend.

## API Service
API calls are centralised in:

```text
src/services/api.js
```

The frontend uses latitude and longitude fields for map data. It does not work directly with raw PostGIS `Point` objects.

## Commands
```zsh
npm install
npm start
npm run build
```

The app expects the backend API to be available at:

```text
http://localhost:5141/api
```
