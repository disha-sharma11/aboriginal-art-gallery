using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.Models;
using AboriginalArtGallery.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Tests;

public class ArtifactServiceTests
{
    [Fact]
    public async Task CreateArtifactAsync_WhenRelatedRecordsExist_CreatesArtifactWithExhibition()
    {
        await using var context = CreateDbContext();
        await SeedRequiredRecordsAsync(context);

        var service = new ArtifactService(context);

        var result = await service.CreateArtifactAsync(
            new ArtifactCreateRequestDto
            {
                Title = "Kulata Shield",
                Description = "Ceremonial shield with carved markings.",
                YearCreated = 1998,
                Material = "Wood",
                ArtType = "Shield",
                ArtStyle = "Carved",
                Era = "Contemporary",
                OriginPlaceName = "South Australia",
                Latitude = -26.0,
                Longitude = 133.0,
                ArtistId = 1,
                AboriginalTribeId = 1,
                ExhibitionId = 1,
            }
        );

        result.Should().NotBeNull();
        result!.Title.Should().Be("Kulata Shield");
        result.ExhibitionName.Should().Be("Living Traditions");
        result.Latitude.Should().Be(-26.0);
        result.Longitude.Should().Be(133.0);
    }

    [Fact]
    public async Task SoftDeleteArtifactAsync_HidesArtifactFromQueries()
    {
        await using var context = CreateDbContext();
        await SeedRequiredRecordsAsync(context);

        context.Artifacts.Add(
            new Artifact
            {
                Id = 1,
                Title = "Hidden Artifact",
                Description = "Artifact used to verify soft delete filters.",
                Material = "Ochre",
                ArtType = "Painting",
                ArtStyle = "Dot",
                Era = "Modern",
                OriginPlaceName = "Northern Territory",
                ArtistId = 1,
                AboriginalTribeId = 1,
            }
        );
        await context.SaveChangesAsync();

        var service = new ArtifactService(context);

        var deleted = await service.SoftDeleteArtifactAsync(1);
        var allArtifacts = await service.GetAllArtifactsAsync();

        deleted.Should().BeTrue();
        allArtifacts.Should().BeEmpty();
        context.Artifacts.IgnoreQueryFilters().Single(a => a.Id == 1).IsDeleted.Should().BeTrue();
    }

    private static async Task SeedRequiredRecordsAsync(AppDbContext context)
    {
        context.AboriginalTribes.Add(
            new AboriginalTribe
            {
                Id = 1,
                Name = "Pitjantjatjara",
                Description = "Aboriginal people of the Central Australian desert.",
                StateOrTerritory = "South Australia",
                OriginRegionName = "APY Lands",
            }
        );

        context.Artists.Add(
            new Artist
            {
                Id = 1,
                FullName = "Tjala Artist",
                Biography = "Community artist for integration tests.",
                Region = "APY Lands",
                AboriginalTribeId = 1,
            }
        );

        context.Exhibitions.Add(
            new Exhibition
            {
                Id = 1,
                Name = "Living Traditions",
                Description = "Seasonal exhibition of living cultural practices.",
                StartDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                Location = "Melbourne",
            }
        );

        await context.SaveChangesAsync();
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
