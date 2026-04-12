FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AboriginalArtGallery.Api/AboriginalArtGallery.Api.csproj AboriginalArtGallery.Api/
RUN dotnet restore AboriginalArtGallery.Api/AboriginalArtGallery.Api.csproj

COPY AboriginalArtGallery.Api/ AboriginalArtGallery.Api/
WORKDIR /src/AboriginalArtGallery.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AboriginalArtGallery.Api.dll"]
